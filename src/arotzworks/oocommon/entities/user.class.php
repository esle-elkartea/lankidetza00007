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
    
    $Id: user.class.php,v 1.3 2006/01/19 09:07:00 luis Exp $
*/    
include_once("scaffold.class.php");
	class user extends scaffold
	{
			function user($id)
			{
					$table = getClassConfig($this, "users_table");
					$this->scaffold($table);
					$this->open($id);
			}	
		
			function onOpen()
			{
					
			}	
		
			function onUpdate()
			{
					$r = new request();
					$this->lastsid =  $r->sid;
					$this->timestamp =  time();	
			}
			
			function onInsert()
			{		
					$this->password = md5($this->password);
			}
	}
?>