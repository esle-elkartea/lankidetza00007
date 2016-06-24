<?php
/*
Webalianza ooCommon NG PHP Framework
(c) 2005 Webalianza T.I. S.L.
Development sponsored and started by
Webalianza T.I. S.L.
Project Lead:   Luis Martin-Santos Garcia <luis@webalianza.com>

This file is part of ooCommon NG.

    ooCommon NG is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    ooCommon is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with ooCommon NG; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
    
    $Id: login.class.php,v 1.3 2006/01/10 08:34:32 luis Exp $
*/    
		class login extends oowidget
		{
			var $username_caption = "Username";
			var $password_caption  = "Password";
			var $realm_caption = "Login";
			var $submit_caption = "Log In";
			var $logout_caption = "Logout";
			var $logged_in_caption ="You are logged in as ";
			var $logged_in_realm_caption="Application";
			var $target;
			var $logouttarget;
			
			
			function login($target,$logouttarget)
			{
				$this->target = $target;
				$this->logouttarget = $logouttarget;
				$this->username_caption = getClassConfig($this, "username_caption");								
				$this->password_caption = getClassConfig($this, "password_caption");
				$this->realm_caption = getClassConfig($this, "realm_caption");
				$this->submit_caption = getClassConfig($this, "submit_caption");
				$this->logout_caption = getClassConfig($this, "logout_caption");
				$this->logged_in_caption = getClassConfig($this, "logged_in_caption");
				$this->logged_in_realm_caption = getClassConfig($this, "logged_in_realm_caption");
				
			}
			
			function drawWidget()
			{
				
				$s = new session();
				if (session::check() != false)
				{
					$w = new box($this->logged_in_realm_caption);
					$w->css_box = "login";
					$w->addWidget(html::write($this->logged_in_caption . $s->user->username));
					$w->addWidget(new button($this->logout_caption,$this->logouttarget."?logout=1"));
					return $w->draw();
				}
				$h = new html();
				$h->html .="
					
					<form method=\"post\" action=\"".$this->target."\">
					<table>
						<tr>
							<td colspan=\"2\">
							".$this->realm_caption."
							</td>
						</tr>
						<tr><td>".$this->username_caption."</td><td><input type=\"text\" name=\"username\"></td></tr>			
						<tr><td>".$this->password_caption."</td><td><input type=\"password\" name=\"password\"></td></tr>
						<tr><td><input type=\"submit\" value=\"".$this->submit_caption."\"></td></tr></table></form>";
					
				$b =  new box ($this->realm_caption,$h);
				$b->css_box = "login";
				return $b->draw();
			}
		}









?>