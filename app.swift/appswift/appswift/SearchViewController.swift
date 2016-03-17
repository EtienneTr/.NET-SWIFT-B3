//
//  SearchViewController.swift
//  appswift
//
//  Created by Supinfo on 17/03/16.
//  Copyright © 2016 Supinfo. All rights reserved.
//

import Foundation
import UIKit
import TwitterKit

class SearchViewController : TWTRTimelineViewController, TWTRTweetViewDelegate {
    override func viewDidLoad() {
        super.viewDidLoad()
        
        let store = Twitter.sharedInstance().sessionStore
        let client = TWTRAPIClient(userID: store.session()?.userID)
        self.dataSource = TWTRSearchTimelineDataSource(searchQuery: UserLog.Search, APIClient: client)
    }
    
    //TweetView
    func tweetView(tweetView: TWTRTweetView, didTapProfileImageForUser user: TWTRUser) {
        
        if(user.userID != UserLog.UserID){
            GotoUserTimeLineController(user.userID)
        }
    }
        
    //navigue vers la timeline
    func GotoUserTimeLineController(userID: String) {
        
        let timelineViewController = self.storyboard!.instantiateViewControllerWithIdentifier("TimelineController") as! TimelineController
        timelineViewController.userID = userID
        timelineViewController.timelineNav = "user"
        self.navigationController!.pushViewController(timelineViewController, animated: true)
    }
}