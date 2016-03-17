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
    var lastID = "0"
    
    override func viewDidLoad() {
        //super.viewDidLoad()
        // Setup the table view
        tableView.estimatedRowHeight = 150
        tableView.rowHeight = UITableViewAutomaticDimension // Explicitly set on iOS 8 if using automatic row height calculation
        tableView.allowsSelection = false
        tableView.registerClass(TWTRTweetTableViewCell.self, forCellReuseIdentifier: tweetTableReuseIdentifier)
        
        //Construction de la Timeline du User voulu. Default = Home du User authentifié
        
        if(userID != "default"){
            CurrentUser = User(userID: self.userID)
        } else {
            CurrentUser = UserLog
        }
        
        if self.timelineNav == "user" {
            getTimeLine("user", user: self.userID, append: false)
        } else {
            getTimeLine("home", user: self.userID, append: false)
        }
        
    }
    
    //MARKS : Override TableView
    override func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.tweetData.count
    }

    
    //construit le Tweet à chaque row
    override func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        let tweet = self.tweetData[indexPath.row]
        let cell = tableView.dequeueReusableCellWithIdentifier(tweetTableReuseIdentifier, forIndexPath: indexPath) as! TWTRTweetTableViewCell
        cell.configureWithTweet(tweet as! TWTRTweet)
        cell.tweetView.delegate = self
        cell.tweetView.showActionButtons = true
        
        //si Retweet = fond
        if tweet.isRetweeted == true {
            cell.subviews[0].subviews[0].backgroundColor = UIColor( red: CGFloat(170/255.0), green: CGFloat(255/255.0), blue: CGFloat(182/255.0), alpha: CGFloat(0.5) )
            //cell.tweetView.layer.backgroundColor = UIColor.greenColor().CGColor
        } else {
            cell.subviews[0].subviews[0].backgroundColor = UIColor.whiteColor()
        }
        
        //Détecte l'autoReload (charger les TWeets suivants
        if(indexPath.row == tableView.numberOfRowsInSection(0) - 1){
            //ID du dernier Tweet pour MaxID
            self.lastID = tweet.tweetID
            if self.timelineNav == "user" {
                getTimeLine("user", user: self.userID, append: true)
            } else {
                getTimeLine("home", user: self.userID, append: true)
            }
        }
        
        return cell
    }
    
    override func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat {
        let tweet = self.tweetData[indexPath.row]
        return TWTRTweetTableViewCell.heightForTweet(tweet as! TWTRTweet, width: CGRectGetWidth(self.view.bounds), showingActions: true)
    }
    
    //construction du Header
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
    
    //fix autoReload size TableView
    override func tableView(tableView: UITableView, estimatedHeightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat {
        let tweet = self.tweetData[indexPath.row]
        return TWTRTweetTableViewCell.heightForTweet(tweet as! TWTRTweet, width: CGRectGetWidth(self.view.bounds), showingActions: true)
    }
    
    //MARKS : Get TimeLine View Home ou User
    func getTimeLine(type: String, user: String, append: Bool){
        
        let store = Twitter.sharedInstance().sessionStore
        let client = TWTRAPIClient(userID: store.session()?.userID)
        
        switch type {
            case "home":
                let statusesShowEndpoint = "https://api.twitter.com/1.1/statuses/home_timeline.json"
                var params = ["count": "20"]
                
                if(append){
                    params["max_id"] = self.lastID
                }
                
                var clientError : NSError?
                
                let request = client.URLRequestWithMethod("GET", URL: statusesShowEndpoint, parameters: params, error: &clientError)
                if (clientError == nil) {
                    
                    client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                        if (connectionError == nil) {
                            do {
                                let json : AnyObject? = try NSJSONSerialization.JSONObjectWithData(data!, options: NSJSONReadingOptions())
                                print(json?.count)
                                if(!append){
                                    self.tweetData = TWTRTweet.tweetsWithJSONArray(json as! [AnyObject])
                                    self.tableView.reloadData()
                                } else {
                                    //self.tweetData.append(TWTRTweet.tweetsWithJSONArray(json as! [AnyObject]))
                                    var dataArray = TWTRTweet.tweetsWithJSONArray(json as! [AnyObject])
                                    dataArray.removeFirst()
                                    var lastSize = self.tweetData.count
                                    self.tweetData.appendContentsOf(dataArray)
                                    self.tableView.beginUpdates()
                                    var Index = [NSIndexPath]()
                                    while lastSize < self.tweetData.count {
                                        Index.append(NSIndexPath(forRow: lastSize, inSection: 0))
                                        lastSize += 1
                                    }
                                    
                                    self.tableView.insertRowsAtIndexPaths(Index, withRowAnimation: .None)
                                    self.tableView.endUpdates()
                                    
                                }
                                
                               // self.tableView.reloadData()
                               // print(self.tweetData)
                            } catch {
                                print("erreur lors du chargement des Tweets")
                            }
                        } else {
                            print(connectionError)
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
                var params = ["user_id": userID]
                
                if(append){
                    params["max_id"] = self.lastID
                }
                
                var clientError : NSError?
                
                let request = client.URLRequestWithMethod("GET", URL: statusesShowEndpoint, parameters: params, error: &clientError)
                if (clientError == nil) {
                    
                    client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                        if (connectionError == nil) {
                            do {
                                let json : AnyObject? = try NSJSONSerialization.JSONObjectWithData(data!, options: NSJSONReadingOptions())
                                
                                if(!append){
                                    self.tweetData = TWTRTweet.tweetsWithJSONArray(json as! [AnyObject])
                                    self.tableView.reloadData()
                                } else {
                                    var dataArray = TWTRTweet.tweetsWithJSONArray(json as! [AnyObject])
                                    dataArray.removeFirst()
                                    var lastSize = self.tweetData.count
                                    self.tweetData.appendContentsOf(dataArray)
                                    self.tableView.beginUpdates()
                                    var Index = [NSIndexPath]()
                                    while lastSize < self.tweetData.count {
                                        Index.append(NSIndexPath(forRow: lastSize, inSection: 0))
                                        lastSize += 1
                                    }
                                    
                                    self.tableView.insertRowsAtIndexPaths(Index, withRowAnimation: .None)
                                    self.tableView.endUpdates()
                                }
                                
                                
                                //print(self.tweetData)
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
    
    //navigue vers la timeline
    func GotoUserTimeLineController(userID: String) {
        
        let timelineViewController = self.storyboard!.instantiateViewControllerWithIdentifier("TimelineController") as! TimelineController
        timelineViewController.userID = userID
        timelineViewController.timelineNav = "user"
        self.navigationController!.pushViewController(timelineViewController, animated: true)
    }
    
    
    //Marks: TweetView = Actions utilisateur sur un Tweet
    
    func tweetView(tweetView: TWTRTweetView, didSelectTweet: TWTRTweet) -> Void {
        let alert = UIAlertController(title: "Tweet Action",
            message: "Vous pouvez effectuer une action sur le tweet : ",
            preferredStyle: UIAlertControllerStyle.Alert
        )
        if(didSelectTweet.author.name != UserLog.Name || didSelectTweet.isRetweet){
            var title = "Retweet"
            if didSelectTweet.isRetweet {
                title = "UnRetweet"
            }
            alert.addAction(UIAlertAction(title: title, style: UIAlertActionStyle.Default, handler: { action in self.Retweet(tweetView, tweet: didSelectTweet) } ))
        } else {
            alert.addAction(UIAlertAction(title: "Supprimer", style: UIAlertActionStyle.Default, handler: { action in self.DeleteTweet(tweetView, tweet: didSelectTweet) } ))
        }
        alert.addAction(UIAlertAction(title: "Répondre", style: UIAlertActionStyle.Default,handler: { action in self.ReplyAction(tweetView, tweet: didSelectTweet) } ))
        alert.addAction(UIAlertAction(title: "Annuler", style: UIAlertActionStyle.Default, handler: nil))
        self.presentViewController(alert, animated: true, completion: nil)
    }
    
    func tweetView(tweetView: TWTRTweetView, didTapProfileImageForUser user: TWTRUser) {
        
        if(user.userID != CurrentUser.UserID){
            GotoUserTimeLineController(user.userID)
        }
    }
    
    
    //Retweet/Unretweet a Tweet
    func Retweet(tweetView: TWTRTweetView, tweet: TWTRTweet) {
        let store = Twitter.sharedInstance().sessionStore
        let client = TWTRAPIClient(userID: store.session()?.userID)
        //retweet
        if(tweet.isRetweeted == false){
        
            let statusesShowEndpoint = "https://api.twitter.com/1.1/statuses/retweet/\(tweet.tweetID).json"
            let params = ["id": tweet.tweetID]
            var clientError : NSError?
        
            let request = client.URLRequestWithMethod("POST", URL: statusesShowEndpoint, parameters: params, error: &clientError)
            if (clientError == nil) {
                client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                    if (connectionError == nil) {
                        print("Retweet")
                        tweetView.backgroundColor = UIColor( red: CGFloat(170/255.0), green: CGFloat(255/255.0), blue: CGFloat(182/255.0), alpha: CGFloat(0.5) )
                    }
                }
                
            }
        }
        //Unretweet
        else {
            
            let statusesShowEndpoint = "https://api.twitter.com/1.1/statuses/unretweet/\(tweet.tweetID).json"
            let params = ["id": tweet.tweetID]
            var clientError : NSError?
            
            let request = client.URLRequestWithMethod("POST", URL: statusesShowEndpoint, parameters: params, error: &clientError)
            if (clientError == nil) {
                client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                    if (connectionError == nil) {
                        print("UnRetweet")
                        tweetView.backgroundColor = UIColor.whiteColor()
                    }
                }
                
            }
        }
    }
    
        func DeleteTweet(tweetView: TWTRTweetView, tweet: TWTRTweet){
            let store = Twitter.sharedInstance().sessionStore
            let client = TWTRAPIClient(userID: store.session()?.userID)
            
            let statusesShowEndpoint = "https://api.twitter.com/1.1/statuses/destroy/\(tweet.tweetID).json"
            let params = ["id": tweet.tweetID]
            var clientError : NSError?
            
            let request = client.URLRequestWithMethod("POST", URL: statusesShowEndpoint, parameters: params, error: &clientError)
            if (clientError == nil) {
                client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                    if (connectionError == nil) {
                        print("Delete")
                        let index = self.tweetData.indexOf({(tweet) -> Bool in
                            return true
                        })
                        self.tweetData.removeAtIndex(index!)
                        //self.tweetData = self.tweetData.filter { return $0 as! TWTRTweet != tweet }
                        self.tableView.reloadData()
                    }
                }
                
            }
        }
    
        func ReplyAction (tweetView: TWTRTweetView, tweet: TWTRTweet){
            let AuthorScreen = tweet.author.screenName
            var inputTextField: UITextField?
            
            let alert = UIAlertController(title: "Reply Tweet",
                message: "Vous pouvez effectuer une action sur le tweet : ",
                preferredStyle: UIAlertControllerStyle.Alert
            )
            alert.addTextFieldWithConfigurationHandler {(textField: UITextField!) in
                textField.text = "@\(AuthorScreen)"
                inputTextField = textField
            }
            alert.addAction(UIAlertAction(title: "Reply", style: UIAlertActionStyle.Default, handler: { action in self.ReplyTweet(tweet, text: inputTextField?.text) } ))
            alert.addAction(UIAlertAction(title: "Annuler", style: UIAlertActionStyle.Default, handler: nil))
            self.presentViewController(alert, animated: true, completion: nil)
            
        }
    
    func ReplyTweet(tweet: TWTRTweet, text : String?){
        if text?.characters.count > 140 {
            print("Error : trop de caractères")
        } else {
            let store = Twitter.sharedInstance().sessionStore
            let client = TWTRAPIClient(userID: store.session()?.userID)
            
            let statusesShowEndpoint = "https://api.twitter.com/1.1/statuses/update.json"
            let params = ["status": text, "in_reply_to_status_id": tweet.tweetID]
            var clientError : NSError?
            
            let request = client.URLRequestWithMethod("POST", URL: statusesShowEndpoint, parameters: params, error: &clientError)
            if (clientError == nil) {
                client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                    if (connectionError == nil) {
                        print("Répondu")
                    }
                }
                
            }
        }
    
    }
    
    
}

//Class for TableView Headet Outlets
class HeaderCellTimeLineController : UITableViewCell {
    
    @IBOutlet var HeaderLabel: UILabel!
    @IBOutlet var ScreenName: UILabel!
    @IBOutlet var ProfileImg: UIImageView!
}

