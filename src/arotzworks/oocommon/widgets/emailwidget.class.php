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
    
    $Id: emailwidget.class.php,v 1.2 2005/12/27 16:25:15 luis Exp $
*/    
	class emailwidget extends oowidget
	{
			var $name;
			var $address;
	
			function check()
			{
				// Taken from php.net/eregi (bobocop at bobocop dot cz)
				// Completely update for match RFC 2822 and RFC 1035
				// http://www.faqs.org/rfcs/rfc2822.html
				// http://www.faqs.org/rfcs/rfc1035.html
				
				
				$atom = '[-a-z0-9!#$%&\'*+/=?^_`{|}~]';    // allowed characters for part before "at" character
				$domain = '([a-z]([-a-z0-9]*[a-z0-9]+)?)'; // allowed characters for part after "at" character
				$regex = '^' . $atom . '+' .        // One or more atom characters.
				'(\.' . $atom . '+)*'.              // Followed by zero or more dot separated sets of one or more atom characters.
				'@'.                                // Followed by an "at" character.
				'(' . $domain . '{1,63}\.)+'.        // Followed by one or max 63 domain characters (dot separated).
				$domain . '{2,63}'.                  // Must be followed by one set consisting a period of two
				'$';  	
				
				return eregi($regex, $this->address);
			}
					
			function onReceive()
			{
				$o = $this->popMailBox();
				if ($o->address) { $this->address = $o->address; }	
			}		
					
			function draw()
			{
				if ($this->check())
				return "<a href=\"mailto:".$this->address."\">".($this->name ? $this->name : $this->address )."</a>";
				return "n/a";
			}	
			
	}