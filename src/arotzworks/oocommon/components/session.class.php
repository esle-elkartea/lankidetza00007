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
    
    $Id: session.class.php,v 1.3 2005/12/27 16:25:15 luis Exp $
*/    		
		class session extends oocomponent
		{
			var $__sessions_table;
			var $__sessions_acl_table;
			var $__sessions_objects_table;
			var $__users_table;
			var $sid;
			var $userid;
			var $user;
			var $objects;
						
			function session($amp = 0)
			{
				if ($amp == -1) return false;
				global $__temp_sid;
				$r = new request();
				if ($__temp_sid)
					$r->sid = $__temp_sid;
				$this->__sessions_table = getClassConfig($this, "sessions_table");
				$this->__sessions_acl_table = getClassConfig($this, "sessions_acl_table");	
				$this->__sessions_objects_table = getClassConfig($this, "sessions_objects_table");
				$this->__users_table = getClassConfig($this, "users_table");
				
				if (!$r->sid) return false;
				if ($r->sid)
					{
						if (!$this->checkSid($r->sid))
						{
							return false;	
						}
					}				
				$this->sid = $r->sid;
				$this->userid = database::quickQuery("SELECT * FROM ".$this->__sessions_table." WHERE sid='".$this->sid."'","iduser");
				$user = new user($this->userid);
				$this->user = $user;
				$this->fetchSessionObjects();
				$this->user->update();
			}
			
			function check()
			{
				$s = new session();
				return $s->checkSid($s->sid);	
			}
			function logout()
			{
				$db = new database();
				$db->query ("DELETE FROM ".$this->__sessions_table." WHERE sid='".$this->sid."'");	
				$s = new session();
			}
			function fetchSessionObjects()
			{
				if (!$this->checkSid()) return false;
				$db = new database();	
				$db->query ("SELECT * FROM ".$this->__sessions_objects_table." WHERE sid='".$this->sid."'");
				for ($i=0;$i<count($db->rows);$i++)
				{
					$o = (base64_decode($db->rows[$i]["o"]));
					$uo = unserialize($o);
					$this->objects[] = $uo; 	
				}
			}	
				
			function checkSid($sid="")
			{
				
				if (!$sid) $sid = $this->sid;
				if (!$sid) return false;
				if (database::quickQuery("SELECT sid FROM ".$this->__sessions_table." WHERE sid='".$sid."'","sid"))
					return true;
				return false;
			}	
						
			function onDrop()
			{
				ooerror(CODE_ERROR,"Session objects cannot be dropped");	
			}
			
			function registerSession()
			{
				$db = new database();
				$sid = md5(uniqid(rand().$this->toString()));
				$db->query ("INSERT INTO ".$this->__sessions_table." (sid,iduser,timestamp) VALUES ('".$sid."',".$this->userid.",".time().");");
				$this->sid = $sid;		
				setcookie ("sid",$sid);
				global $__temp_sid;
				$__temp_sid = $sid;
				return $sid;
			}
			
			function login($username,$password)
			{
				$db = new database();
				if ($username && $password)
				{
					$db->query ("SELECT * FROM ".getClassConfig(new user(0), "users_table")." WHERE username='".$username."' AND password='".md5($password)."' LIMIT 1");
					if ($db->rows[0]["username"] == $username && $db->rows[0]["password"] == md5($password))
					{
						$this->userid = $db->rows[0]["id"];
						$u = new user($this->userid);
						$this->user = $u;
						$this->registerSession();
						return true;	
					}
				}
				return false;
			}
			
			function acl($object,$method)
			{
				if (!$this->checkSid()) return false;
				$db = new database();
				$db->query ("SELECT * FROM ".$this->__sessions_acl_table." WHERE iduser=".$this->userid. "AND object='".get_class($object)."' AND method='".$method."'");
				if ($db->rows[0]["object"] == get_class($object) && $db->rows[0]["method"] == $method)
					return true;
				return false;	
			}
			
			function onReceive()
			{
				if (!$this->checkSid()) return false;
				$db = new database();
				$o = $this->popMailBox();
				$os = serialize($o);
				if ($os && $o)
				{
					$db->query ("INSERT INTO ".$this->__sessions_objects_table." (sid,o) VALUES ('".$this->sid."', '".base64_encode(serialize($os))."');");
					return true;
				}	
				return false;
			}						
			
			function __install()
			{
				// Toma kludge!	
				$db = new database();
				$db->query ("SHOW TABLES");
				$exists = false;
				$__sessions_table = getClassConfig(new session(-1), "sessions_table");
				$__sessions_acl_table = getClassConfig(new session(-1), "sessions_acl_table");	
				$__sessions_objects_table = getClassConfig(new session(-1), "sessions_objects_table");
				$__users_table = getClassConfig(new session(-1), "users_table");
				
				for ($i=0;$i<count($db->rows);$i++)
				{
					if ($db->rows[$i]["Tables_in_".$db->mysql_database] == $__users_table)
					{
						$exists = true;
					}
				}
				
				if ($exists == true) return;
				$db->query("
				CREATE TABLE ".$__sessions_table." (id int(10) unsigned not null auto_increment,
				sid varchar(32),timestamp int(12),iduser int(10), PRIMARY KEY(id));"); 

				$db->query("
				CREATE TABLE ".$__users_table." (id int(10) unsigned not null auto_increment,
				username varchar(16), password varchar(32), misc text, timestamp int(12),lastsid varchar(32), PRIMARY KEY(id));"); 

				$db->query("
				CREATE TABLE ".$__sessions_objects_table." (id int(10) unsigned not null auto_increment,
				sid varchar(32), o text, PRIMARY KEY(id));"); 

				$db->query("
				CREATE TABLE ".$__sessions_acl_table." (id int(10) unsigned not null auto_increment,
				iduser int(10), object varchar(32), method varchar(32), PRIMARY KEY(id));"); 
			}
		}
		if (@$_REQUEST["OOCOMMON_COPYRIGHT"]){echo "<html><body><pre>ooCommonNG (c) 2005-2006 Webalianza T.I. S.L.\nLicensed under the GNU General Public License</pre></body></html>";die();}