//
//  LoginViewController.swift
//  appswift
//
//  Created by Supinfo on 10/03/16.
//  Copyright © 2016 Supinfo. All rights reserved.
//

import Foundation
import UIKit
import TwitterKit

class LoginViewController : UIViewController {
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.navigationItem.hidesBackButton = true;
        //User is log : affiche profil
        if let _ = Twitter.sharedInstance().sessionStore.session() {
            
            GotoProfilController()
        }
        else {
            //affiche le bouton de log
            let logInButton = TWTRLogInButton { (session, error) in
                
                if let unwrappedSession = session {
                    /*let alert = UIAlertController(title: "Logged In",
                        message: "User \(unwrappedSession.userName) has logged in",
                        preferredStyle: UIAlertControllerStyle.Alert
                    )
                    alert.addAction(UIAlertAction(title: "OK", style: UIAlertActionStyle.Default, handler: nil))
                    self.presentViewController(alert, animated: true, completion: self.GotoProfilController)*/
                    
                    self.GotoProfilController()
                    
                } else {
                    NSLog("Login error: %@", error!.localizedDescription);
                }
            }
            
            // TODO: Change where the log in button is positioned in your view
            logInButton.center = self.view.center
            self.view.addSubview(logInButton)
            
        }
    }
    
    func GotoProfilController() {
        
        let profilViewController = self.storyboard!.instantiateViewControllerWithIdentifier("ViewController")
        
        /*self.navigationController!.pushViewController(profilViewController, animated: true)*/
        self.navigationController?.setViewControllers([profilViewController], animated: true)
    }
}