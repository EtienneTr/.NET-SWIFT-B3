//
//  TimelineController.swift
//  appswift
//
//  Created by Supinfo on 10/03/16.
//  Copyright © 2016 Supinfo. All rights reserved.
//

import Foundation
import TwitterKit

class TimelineController : UITableViewController, TWTRTweetViewDelegate {
    
    
    var tweetData : [AnyObject] = []
    let tweetTableReuseIdentifier = "TweetCell"
    var userID = "default"
    var timelineNav = "home"
    var CurrentUser = UserLog
    var buttonPath : [NSIndexPath] = []
    
    override func viewDidLoad() {
        //super.viewDidLoad()
        // Setup the table view
        tableView.estimatedRowHeight = 150
        tableView.rowHeight = UITableViewAutomaticDimension // Explicitly set on iOS 8 if using automatic row height calculation
        tableView.allowsSelection = false
        tableView.registerClass(TWTRTweetTableViewCell.self, forCellReuseIdentifier: tweetTableReuseIdentifier)
        
        
        /*let store = Twitter.sharedInstance().sessionStore
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
            
        }*/
        
        if(userID != "default"){
            CurrentUser = User(userID: self.userID)
        } else {
            CurrentUser = UserLog
        }
        
        if self.timelineNav == "user" {
            getTimeLine("user", user: self.userID)
        } else {
            getTimeLine("home", user: self.userID)
        }
        
    }
    
    //MARKS : Override TableView
    override func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.tweetData.count
    }
    
    /*override func tableView(tableView: UITableView, willDisplayCell cell: UITableViewCell, forRowAtIndexPath indexPath: NSIndexPath) {
        if !buttonPath.contains(indexPath){
            var currentShare = UIButton()
            var todo = true
            for subview in cell.subviews[0].subviews[0].subviews as [UIView] {
                if todo{
                    if let buttonView = subview as? UIButton {
                        currentShare = buttonView
                        todo = false
                        break
                    }
                }
            }
            print(currentShare)
            print(currentShare.center)
            
            let RTButton = UIButton(frame: currentShare.frame/*CGRectMake(currentShare.frame.minX, currentShare.frame.minY, 40, 30)*/)
            RTButton.frame = CGRectMake(0, 0, 40, cell.frame.size.height - RTButton.frame.size.height)
            //RTButton.center.x = currentShare.center.x
            //RTButton.center.y = currentShare.center.y
            RTButton.layer.backgroundColor = UIColor.redColor().CGColor
            //RTButton.frame.origin = CGPoint(x: currentShare.frame.midX + 20, y: currentShare.frame.midY)
            // RTButton.center.y = RTButton.center.y + 50
            
            RTButton.setTitle("RT", forState: .Normal)
            
            print(RTButton)
            print(RTButton.center)
            print("%%%%%%%%%%")
            cell.subviews[0].subviews[0].addSubview(RTButton)
            
            buttonPath.append(indexPath)
        }
        
    }*/
    
    override func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        let tweet = self.tweetData[indexPath.row]
        let cell = tableView.dequeueReusableCellWithIdentifier(tweetTableReuseIdentifier, forIndexPath: indexPath) as! TWTRTweetTableViewCell
        cell.configureWithTweet(tweet as! TWTRTweet)
        cell.tweetView.delegate = self
        cell.tweetView.showActionButtons = true
        
        if tweet.isRetweeted == true {
            cell.subviews[0].backgroundColor = UIColor.greenColor()
        }
        
        /*let RTView = UIView()
        let RTLabel = UILabel(frame: CGRectMake(160, 121, 40, 30))
        RTLabel.center = CGPointMake(160, 284)
        RTLabel.textAlignment = NSTextAlignment.Center
        RTLabel.text = "RT"
        RTView.addSubview(RTLabel)*/
        
        //cell.addSubview(RTView)
        //print(cell.subviews[0].subviews[0].subviews)
        //let currentShare = cell.subviews[0].subviews[0].subviews[1] as! UIButton
        /*if !buttonPath.contains(indexPath){
        var currentShare = UIButton()
        var todo = true
        for subview in cell.subviews[0].subviews[0].subviews as [UIView] {
            if todo{
                if let buttonView = subview as? UIButton {
                    currentShare = buttonView
                    todo = false
                    break
                }
            }
        }
        print(currentShare)
        print(currentShare.center)
        
        let RTButton = UIButton(frame: currentShare.frame/*CGRectMake(currentShare.frame.minX, currentShare.frame.minY, 40, 30)*/)
        //RTButton.center.x = currentShare.center.x
        //RTButton.center.y = currentShare.center.y
        RTButton.layer.backgroundColor = UIColor.redColor().CGColor
        //RTButton.frame.origin = CGPoint(x: currentShare.frame.midX + 20, y: currentShare.frame.midY)
       // RTButton.center.y = RTButton.center.y + 50
        
        RTButton.setTitle("RT", forState: .Normal)
        
        print(RTButton)
        print(RTButton.center)
        print("%%%%%%%%%%")
        cell.subviews[0].subviews[0].addSubview(RTButton)
        
        buttonPath.append(indexPath)
        }*/
        return cell
    }
    
    override func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat {
        let tweet = self.tweetData[indexPath.row]
        return TWTRTweetTableViewCell.heightForTweet(tweet as! TWTRTweet, width: CGRectGetWidth(self.view.bounds)) + 15
    }
    
   override func tableView(tableView: UITableView, viewForHeaderInSection section: Int) -> UIView? {
    
        //Build Header Cell with current user
        let cell = tableView.dequeueReusableCellWithIdentifier("HeaderCell") as! HeaderCellTimeLineController
        cell.ScreenName.text = "@\(CurrentUser.ScreenName)"
        cell.HeaderLabel.text = CurrentUser.Name
    
        let url = NSURL(string: CurrentUser.ImageURL)
        if let data = NSData(contentsOfURL: url!) {
            cell.ProfileImg.image = UIImage(data: data)
        }
    
        cell.ProfileImg.layer.cornerRadius = 8.0
        cell.ProfileImg.clipsToBounds = true
        cell.ProfileImg.layer.borderWidth = 2.0
        cell.ProfileImg.layer.borderColor = UIColor.lightGrayColor().CGColor
    
        return cell
    }
    
    override func tableView(tableView: UITableView, heightForHeaderInSection section: Int) -> CGFloat {
        return 65
    }
    
    //MARKS : Get TimeLine View
    func getTimeLine(type: String, user: String){
        
        let store = Twitter.sharedInstance().sessionStore
        let client = TWTRAPIClient(userID: store.session()?.userID)
        
        switch type {
            case "home":
                let statusesShowEndpoint = "https://api.twitter.com/1.1/statuses/home_timeline.json"
                let params = ["count": "20"]
                var clientError : NSError?
                
                let request = client.URLRequestWithMethod("GET", URL: statusesShowEndpoint, parameters: params, error: &clientError)
                if (clientError == nil) {
                    
                    client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                        if (connectionError == nil) {
                            do {
                                let json : AnyObject? = try NSJSONSerialization.JSONObjectWithData(data!, options: NSJSONReadingOptions())
                                self.tweetData = TWTRTweet.tweetsWithJSONArray(json as! [AnyObject])
                                
                                self.tableView.reloadData()
                               // print(self.tweetData)
                            } catch {
                                print("erreur lors du chargement des Tweets")
                            }
                        }
                    }
                }
                break
            case "user":
                var userID = ""
                if(user == "default"){
                    userID = store.session()!.userID
                } else {
                    userID = user
                    self.userID = "default"
                }
                let statusesShowEndpoint = "https://api.twitter.com/1.1/statuses/user_timeline.json"
                let params = ["user_id": userID]
                var clientError : NSError?
                
                let request = client.URLRequestWithMethod("GET", URL: statusesShowEndpoint, parameters: params, error: &clientError)
                if (clientError == nil) {
                    
                    client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                        if (connectionError == nil) {
                            do {
                                let json : AnyObject? = try NSJSONSerialization.JSONObjectWithData(data!, options: NSJSONReadingOptions())
                                self.tweetData = TWTRTweet.tweetsWithJSONArray(json as! [AnyObject])
                                
                                self.tableView.reloadData()
                                print(self.tweetData)
                            } catch {
                                print("erreur lors du chargement des Tweets")
                            }
                        }
                    }
                }
                break
            default:
                break
        }
    }
    
    func GotoUserTimeLineController(userID: String) {
        
        let timelineViewController = self.storyboard!.instantiateViewControllerWithIdentifier("TimelineController") as! TimelineController
        timelineViewController.userID = userID
        timelineViewController.timelineNav = "user"
        self.navigationController!.pushViewController(timelineViewController, animated: true)
    }
    
    //Marks: TweetView
    
    func tweetView(tweetView: TWTRTweetView, didSelectTweet: TWTRTweet) -> Void {
        let alert = UIAlertController(title: "Retweet",
            message: "Voulez-vous Retweet ?",
            preferredStyle: UIAlertControllerStyle.Alert
        )
        alert.addAction(UIAlertAction(title: "Oui", style: UIAlertActionStyle.Default, handler: { action in self.Retweet(didSelectTweet) } ))
        alert.addAction(UIAlertAction(title: "Non", style: UIAlertActionStyle.Default, handler: nil))
        self.presentViewController(alert, animated: true, completion: nil)
    }
    
    func tweetView(tweetView: TWTRTweetView, didTapProfileImageForUser user: TWTRUser) {
        
        if(user.userID != CurrentUser.UserID){
            GotoUserTimeLineController(user.userID)
        }
    }
    
    func Retweet(tweet: TWTRTweet) {
        
        if(tweet.isRetweeted == false){
        let store = Twitter.sharedInstance().sessionStore
        let client = TWTRAPIClient(userID: store.session()?.userID)
        
        let statusesShowEndpoint = "https://api.twitter.com/1.1/statuses/retweet/\(tweet.tweetID).json"
        let params = ["id": tweet.tweetID]
        var clientError : NSError?
        
        let request = client.URLRequestWithMethod("POST", URL: statusesShowEndpoint, parameters: params, error: &clientError)
        if (clientError == nil) {
            print("Retweet")
        }
        }
    }
    
    
    
}

class HeaderCellTimeLineController : UITableViewCell {
    
    @IBOutlet var HeaderLabel: UILabel!
    @IBOutlet var ScreenName: UILabel!
    @IBOutlet var ProfileImg: UIImageView!
}
