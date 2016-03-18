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
var Args : String = ""

class ViewController: UIViewController, UISearchBarDelegate {
    
    
    @IBOutlet var LoginUserName: UILabel!
    @IBOutlet var LabelsView: UIView!
    @IBOutlet var ScreenNameUser: UIButton!
    
    
    @IBOutlet var UserProfilView: UIView!
    @IBOutlet var UserImageParent: UIView!
    @IBOutlet var UserImage: UIImageView!
    
    @IBOutlet var LogoutButton: UIButton!
    @IBOutlet var UserView: UIStackView!
    
    @IBOutlet var SearchBar: UISearchBar!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.navigationItem.hidesBackButton = true
        SearchBar.delegate = self
        
        let gesture = UIGestureRecognizer(target: self, action: Selector("ProfilViewTouch"))
        UserImage.userInteractionEnabled = true
        UserImage.addGestureRecognizer(gesture)
        
        //User is log : affiche profil
        if let _ = Twitter.sharedInstance().sessionStore.session() {
            
            let session = Twitter.sharedInstance().sessionStore.session()
            let client = TWTRAPIClient(userID: session!.userID)
            
            client.loadUserWithID(session!.userID) { (user, error) -> Void in
                if let user = user {
                    UserLog.ScreenName = user.screenName
                    UserLog.Name = user.name
                    UserLog.ImageURL = user.profileImageLargeURL
                    UserLog.UserID = user.userID
                    
                    self.getUserFullJson()
                    self.updateInterface()
                    
                }
            }
            
        } else {
            GotoConnectionController()
        }
        
    }
    
    func updateInterface(){
        
        self.LoginUserName.text = "Bienvenue \(UserLog.Name)"
        self.ScreenNameUser.setTitle("@\(UserLog.ScreenName)", forState: .Normal)
        
        let url = NSURL(string: UserLog.ImageURL)
        if let data = NSData(contentsOfURL: url!) {
            self.UserImage.image = UIImage(data: data)
        }
        self.UserImage.layer.cornerRadius = 10.0
        self.UserImage.clipsToBounds = true
        self.UserImage.layer.borderWidth = 3.0
        self.UserImage.layer.borderColor = UIColor.whiteColor().CGColor
        
        //center elements
        //self.UserImage.center = CGPointMake(UserImageParent.frame.size.width  / 2, UserImageParent.frame.size.height / 2);
        //self.LoginUserName.center = CGPointMake(LabelsView.frame.size.width  / 2, LabelsView.frame.size.height / 2);
        //self.ScreenNameUser.center = CGPointMake(LabelsView.frame.size.width  / 2, LabelsView.frame.size.height / 2);
        
    }
    
    func getUserFullJson(){
        
        
        let store = Twitter.sharedInstance().sessionStore
        let client = TWTRAPIClient(userID: store.session()?.userID)
        
        let statusesShowEndpoint = "https://api.twitter.com/1.1/users/profile_banner.json"
        let params = ["user_id": store.session()!.userID]
        var clientError : NSError?
        
        let request = client.URLRequestWithMethod("GET", URL: statusesShowEndpoint, parameters: params, error: &clientError)
        if (clientError == nil) {
            
            client.sendTwitterRequest(request) { (response, data, connectionError) -> Void in
                if (connectionError == nil) {
                    do {
                        let json : AnyObject? = try NSJSONSerialization.JSONObjectWithData(data!, options: NSJSONReadingOptions())
                        //let userData =
                        //print(json)
                        var mobileBanner = ""
                        if let datas = json as? NSDictionary {
                            if let sizes = datas["sizes"] as? NSDictionary{
                                if let size = sizes["mobile"] as? NSDictionary{
                                    if let url = size["url"] as? String {
                                        mobileBanner = url
                                    }
                                }
                            }
                        }
                        
                        UserLog.BannerImg = mobileBanner
                        
                        //update interface
                        if mobileBanner != "" {
                            let BannerUrl = NSURL(string: UserLog.BannerImg)
                            if let data = NSData(contentsOfURL: BannerUrl!) {
                                
                                //draw center image
                                UIGraphicsBeginImageContext(self.UserImageParent.frame.size)
                                
                                //let ctx = UIGraphicsGetCurrentContext();
                                //CGContextSetAlpha(ctx, 0.5)
                                UIImage(data: data)?.drawInRect(self.UserImageParent.bounds)
                                
                                let image: UIImage = UIGraphicsGetImageFromCurrentImageContext()
                                UIGraphicsEndImageContext()
                                

                                let imgBanner = image
                                
                                self.UserImageParent.backgroundColor = UIColor(patternImage: imgBanner).colorWithAlphaComponent(0.5)
                                self.UserImageParent.layer.cornerRadius = 10
                                self.UserImage.clipsToBounds = true
                            }
                        }
                        
                    } catch {
                        print("Erreur de chargement bannière")
                    }
                }
            }
        }
        
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
    
    //MARK: Actions
    @IBAction func LogoutAction(sender: AnyObject) {
        
        let store = Twitter.sharedInstance().sessionStore
        
        if let userID = store.session()?.userID {
            store.logOutUserID(userID)
        }
        
        GotoConnectionController()
    }
    
    @IBAction func HomeTLAction(sender: AnyObject) {
        GotoTimeLineController("user")
        //view the User Profil TimeLine
    }
    
    func ProfilViewTouch(){
        GotoTimeLineController("user")
    }
    
    @IBAction func ProfilAction(sender: UIButton) {
        GotoTimeLineController("user")
    }
    
    func GotoConnectionController() {
        
        let loginViewController = self.storyboard!.instantiateViewControllerWithIdentifier("LoginViewController") as! LoginViewController
        
        self.navigationController!.pushViewController(loginViewController, animated: true)
    }
    
    func GotoTimeLineController(tlAction: String) {
        
        let timelineViewController = self.storyboard!.instantiateViewControllerWithIdentifier("TimelineController") as! TimelineController
        timelineViewController.timelineNav = tlAction   
        self.navigationController!.pushViewController(timelineViewController, animated: true)
    }
    
    func GotoSearchViewController() {
        
        let searchViewController = self.storyboard!.instantiateViewControllerWithIdentifier("SearchView") as! SearchViewController
        
        self.navigationController!.pushViewController(searchViewController, animated: true)
    }
    
    //MArk : Search Delegate
    func searchBarSearchButtonClicked(searchBar: UISearchBar) {
        if let text = SearchBar.text {
            UserLog.Search = text
            GotoSearchViewController()
        }
    }
    
    
    
}

