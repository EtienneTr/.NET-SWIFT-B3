//
//  ViewController.swift
//  appswift
//
//  Created by Supinfo on 07/03/16.
//  Copyright © 2016 Supinfo. All rights reserved.
//

import UIKit
import TwitterKit

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

class TimelineController : TWTRTimelineViewController  {
    
    override func viewDidLoad() {
        super.viewDidLoad()

        let store = Twitter.sharedInstance().sessionStore
        let client = TWTRAPIClient(userID: store.session()?.userID)
        
        //let client = TWTRAPIClient()
        self.dataSource = TWTRUserTimelineDataSource(screenName: "EdwinnSsOff", APIClient: client)
        
        
        self.showTweetActions = true
        
        /*let statusesShowEndpoint = "https://api.twitter.com/1.1/statuses/user_timeline.json"
        let params = ["id": "20"]
        var clientError : NSError?
        
        let request = Twitter.sharedInstance().APIClient.URLRequestWithMethod("GET", URL: statusesShowEndpoint, parameters: params, error: &clientError)
        if (clientError == nil) {
            
            client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                if (connectionError == nil) {
                    var jsonError : NSError?
                    do {
                        let json : AnyObject? = try NSJSONSerialization.JSONObjectWithData(data!, options: NSJSONReadingOptions())
                    } catch {
                        jsonError = nil
                    }
                }
            }
            
        }*/

    }

}

