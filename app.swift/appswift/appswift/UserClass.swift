//
//  UserClass.swift
//  appswift
//
//  Created by Supinfo on 08/03/16.
//  Copyright © 2016 Supinfo. All rights reserved.
//

import Foundation
import UIKit
import TwitterKit

class User {
    var Name : String = "Guest"
    var ScreenName : String = "@Guest"
    var ImageURL : String = ""
    var BannerImg : String = ""
    var StatusCount = "0"
    var Followers = "0"
    var Following = "0"
    
    
    func getUserFullJson(){
        
        let store = Twitter.sharedInstance().sessionStore
        let client = TWTRAPIClient(userID: store.session()?.userID)
        
        let statusesShowEndpoint = "https://api.twitter.com/1.1/users/lookup.json"
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
                        print(json)
                        if let datas = json as? NSArray {
                            if let data = datas[0] as? NSDictionary{
                                if let followers = data["followers_count"] as? String {
                                    UserLog.Followers = followers
                                }
                                if let friends = data["friends_count"] as? String {
                                    UserLog.Following = friends
                                }
                                if let status = data["statuses_count"] as? String {
                                    UserLog.StatusCount = status
                                }
                            }
                        }
                        
                    } catch {
                        print("Erreur de chargement bannière")
                    }
                }
            }
        }
        
    }
}