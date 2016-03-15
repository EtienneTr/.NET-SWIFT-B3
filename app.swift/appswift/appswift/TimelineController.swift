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
        if Args == "user"{
            getTimeLine("user")
            Args = ""
        } else {
            getTimeLine("home")
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
        /*let cell = tableView.dequeueReusableCellWithIdentifier("Cells", forIndexPath: indexPath) as! CellTimeLineController
        cell.UserName = tweet.UserName
        cell.UserScreenName = tweet.UserScreenName
        cell.UserImg = tweet.UserImg
        cell.TweetText = tweet.TweetText*/
        cell.tweetView.showActionButtons = true
        
        //cell.tweetView.delegate?.tweetView!(cell.tweetView, didSelectTweet: delTweet)
        return cell
    }
    
    func tweetView(tweetView: TWTRTweetView, didSelectTweet: TWTRTweet) -> Void {
        print("Tweet sélectionné")
    }
    
    override func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat {
        let tweet = self.tweetData[indexPath.row]
        return TWTRTweetTableViewCell.heightForTweet(tweet as! TWTRTweet, width: CGRectGetWidth(self.view.bounds))
    }
    
    /*override func tableView(tableView: UITableView, titleForHeaderInSection section: Int) -> String? {
    return "User"
    }**/
    
    //override func tableView(tableView: UITableView, viewForHeaderInSection section: Int) -> UIView? {
    // Here, we use NSFetchedResultsController
    // And we simply use the section name as title
    //let currSection = fetchedResultsController.sections?[section]
    //let title = currSection!.name
    
        
    //}
    
   override func tableView(tableView: UITableView, viewForHeaderInSection section: Int) -> UIView? {
        /*if UserLog.ImageURL != "" {
            let url = NSURL(string: UserLog.ImageURL)
            if let data = NSData(contentsOfURL: url!) {
                headerCell.backgroundView = UIImageView(image: UIImage(data: data))
            }
        }*/
        let cell = tableView.dequeueReusableCellWithIdentifier("HeaderCell") as! HeaderCellTimeLineController
        cell.ScreenName.text = "@\(UserLog.ScreenName)"
        cell.HeaderLabel.text = UserLog.Name
    
        let url = NSURL(string: UserLog.ImageURL)
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
    
    
    func getTimeLine(type: String){
        
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
                            var jsonError : NSError?
                            do {
                                let json : AnyObject? = try NSJSONSerialization.JSONObjectWithData(data!, options: NSJSONReadingOptions())
                                self.tweetData = TWTRTweet.tweetsWithJSONArray(json as! [AnyObject])
                                
                                self.tableView.reloadData()
                               // print(self.tweetData)
                            } catch {
                                jsonError = nil
                            }
                        }
                    }
                }
                break
            case "user":
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
                break
            default:
                break
        }
    }
    
    
}

class HeaderCellTimeLineController : UITableViewCell {
    
    @IBOutlet var HeaderLabel: UILabel!
    @IBOutlet var ScreenName: UILabel!
    @IBOutlet var ProfileImg: UIImageView!
}

class CellTimeLineController : UITableViewCell {
    
    @IBOutlet var UserImg: UIImageView!
    @IBOutlet var UserName: UIButton!
    @IBOutlet var UserScreenName: UIButton!
    @IBOutlet var TweetText: UILabel!
    
}