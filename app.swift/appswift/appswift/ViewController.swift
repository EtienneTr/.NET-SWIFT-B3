//
//  ViewController.swift
//  appswift
//
//  Created by Supinfo on 07/03/16.
//  Copyright © 2016 Supinfo. All rights reserved.
//

import UIKit
import TwitterKit

var UserLog = User()

class ViewController: UIViewController {
    
    @IBOutlet weak var LoginUserName: UILabel!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        if let _ = Twitter.sharedInstance().sessionStore.session() {
            
            let session = Twitter.sharedInstance().sessionStore.session()
            let client = TWTRAPIClient(userID: session!.userID)
            
            client.loadUserWithID(session!.userID) { (user, error) -> Void in
                if let user = user {
                    
                    self.LoginUserName.text = "Bienvenue @\(user.screenName)"
                    UserLog.Name = user.screenName
                    UserLog.ImageURL = user.profileImageURL
                }
            }
            
        } else {
        
            let logInButton = TWTRLogInButton { (session, error) in
               
                if let unwrappedSession = session {
                    let alert = UIAlertController(title: "Logged In",
                        message: "User \(unwrappedSession.userName) has logged in",
                        preferredStyle: UIAlertControllerStyle.Alert
                    )
                    alert.addAction(UIAlertAction(title: "OK", style: UIAlertActionStyle.Default, handler: nil))
                    self.presentViewController(alert, animated: true, completion: nil)
                    
                    UserLog.Name = unwrappedSession.userName
                    
                } else {
                    NSLog("Login error: %@", error!.localizedDescription);
                }
            }
        
            // TODO: Change where the log in button is positioned in your view
            logInButton.center = self.view.center
            self.view.addSubview(logInButton)
        
        }
        // Do any additional setup after loading the view, typically from a nib.
    }
    

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }

    // MARK: Actions
    
    @IBAction func tweetAction(sender: AnyObject) {
        
        if let _ = Twitter.sharedInstance().sessionStore.session() {
            
            let composer = TWTRComposer()
            
            composer.setText("just tweet from my #swift App")
            //composer.setImage(UIImage(named: "fabric"))
            
            // Called from a UIViewController
            composer.showFromViewController(self) { result in
                if (result == TWTRComposerResult.Cancelled) {
                    //message cancel
                    let alert = UIAlertController(title: "Tweet",
                        message: "Tweet cancelling",
                        preferredStyle: UIAlertControllerStyle.Alert
                    )
                    alert.addAction(UIAlertAction(title: "OK", style: UIAlertActionStyle.Default, handler: nil))
                    self.presentViewController(alert, animated: true, completion: nil)
                }
                else {
                    //message ok
                    let alert = UIAlertController(title: "Tweet",
                        message: "Tweet has been send !",
                        preferredStyle: UIAlertControllerStyle.Alert
                    )
                    alert.addAction(UIAlertAction(title: "OK", style: UIAlertActionStyle.Default, handler: nil))
                    self.presentViewController(alert, animated: true, completion: nil)
                }
            }
        }
    }

}

class TimelineController : UITableViewController, TWTRTweetViewDelegate {
    
    
    var tweetData : [AnyObject] = []
    let tweetTableReuseIdentifier = "TweetCell"
    
    override func viewDidLoad() {
        //super.viewDidLoad()
        // Setup the table view
        tableView.estimatedRowHeight = 150
        tableView.rowHeight = UITableViewAutomaticDimension // Explicitly set on iOS 8 if using automatic row height calculation
        tableView.allowsSelection = false
        tableView.registerClass(TWTRTweetTableViewCell.self, forCellReuseIdentifier: tweetTableReuseIdentifier)
        
        
        let store = Twitter.sharedInstance().sessionStore
        let client = TWTRAPIClient(userID: store.session()?.userID)
        
        //let client = TWTRAPIClient()
        /*let dataSrc = TWTRUserTimelineDataSource(screenName: UserLog.Name, APIClient: client)
        self.dataSource = dataSrc
        
        self.showTweetActions = true*/
        
        let statusesShowEndpoint = "https://api.twitter.com/1.1/statuses/user_timeline.json"
        let params = ["user_id": store.session()!.userID]
        var clientError : NSError?
        
        let request = client.URLRequestWithMethod("GET", URL: statusesShowEndpoint, parameters: params, error: &clientError)
        if (clientError == nil) {
            
            client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                if (connectionError == nil) {
                    var jsonError : NSError?
                    do {
                        let json : AnyObject? = try NSJSONSerialization.JSONObjectWithData(data!, options: NSJSONReadingOptions())
                        self.tweetData = TWTRTweet.tweetsWithJSONArray(json as! [AnyObject])
                        
                        self.tableView.reloadData()
                        print(self.tweetData)
                    } catch {
                        jsonError = nil
                    }
                }
            }
            
        }
        
        

    }
    
    override func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.tweetData.count
    }
    
    override func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        let tweet = self.tweetData[indexPath.row]
        let cell = tableView.dequeueReusableCellWithIdentifier(tweetTableReuseIdentifier, forIndexPath: indexPath) as! TWTRTweetTableViewCell
        cell.configureWithTweet(tweet as! TWTRTweet)
        cell.tweetView.delegate = self
        
        return cell
    }
    
    override func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat {
        let tweet = self.tweetData[indexPath.row]
        return TWTRTweetTableViewCell.heightForTweet(tweet as! TWTRTweet, width: CGRectGetWidth(self.view.bounds))
    }

    /*override func tableView(tableView: UITableView, titleForHeaderInSection section: Int) -> String? {
        return "User"
    }**/
    
    /*override func tableView(tableView: UITableView, viewForHeaderInSection section: Int) -> UIView? {
        // Here, we use NSFetchedResultsController
        // And we simply use the section name as title
        //let currSection = fetchedResultsController.sections?[section]
        //let title = currSection!.name
        
        // Dequeue with the reuse identifier
        let cell = self.tableView.dequeueReusableHeaderFooterViewWithIdentifier("TableSectionHeader")
        //let header = cell as! TableSectionHeader
        header.titleLabel.text = title
        
        return cell
    }*/
    
    override func tableView(tableView: UITableView, viewForHeaderInSection section: Int) -> UIView? {
        let headerCell = tableView.dequeueReusableCellWithIdentifier("HeaderCell") as! CustomHeaderCell
        //headerCell.backgroundColor = UIColor.cyanColor()
        headerCell.headerLabel.text = UserLog.Name
        
        let url = NSURL(string: UserLog.ImageURL)
        let data = NSData(contentsOfURL: url!) //make sure your image in this url does exist, otherwise unwrap in a if let check
        headerCell.backgroundView = UIImageView(image: UIImage(data: data!))
        
        
        return headerCell
    }
    
    override func tableView(tableView: UITableView, heightForHeaderInSection section: Int) -> CGFloat {
        return 80
    }
}
