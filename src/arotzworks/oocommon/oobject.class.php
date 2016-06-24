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
    
    $Id: oobject.class.php,v 1.2 2005/12/27 16:25:15 luis Exp $
*/    
	class oobject {
		
		var $__lock;
		
		function toString()
		{
			return get_class($this);
		}	
	
		function fromObject(&$o)
		{
			foreach ($this as $k=>$v)
			{
				if (strpos($k,"__")===false && $o->$k != "") $this->$k = $o->$k;	
			}	
		}		

		function drop ($object)
			{
			
			if (!is_object($object)) ooerror(403, "Hey! That is not an object!<br />This is an application <b>Coding Error</b>, you cannot drop anything that is not an object to an object.<br />See the backtrace in order to find the offending line. If you cannot fix it, tell your vendor or programmer to fix this, attaching this message. He/She will understand it.");
			$class = get_class($object);
			debug ($this,"<font color=\"blue\">Received object ".$object->toString()."</font>");
			$object->ondrop();
			for ($i=0;$i<count($this->__accepts);$i++)
			{
				if ($class == $this->accepts[$i] || get_parent_class($object) == $this->accepts[$i]) 
				$ok = true;	
			}		
			for($i=0;$i<count($this->__mailbox);$i++)
			{
				if ($this->__mailbox[$i] == "")
					{
						$this->__mailbox[$i] = $object;
						$this->onReceive();
						return;
					}		
			}
			$this->__mailbox[] = $object;
			$this->onReceive();
			return;
		}
			
		function onReceive()
		{
			$o =&$this->__mailbox[count($this->__mailbox) -1];
			debug ($this,"<font color=\"orange\">Unhandled reception of object ".$o->toString()."</font>");
		
		}
		function popMailBox()
		{
			$object = $this->__mailbox[0];
			for ($i=0;$i<count($this->__mailbox);$i++)
			{
						$this->__mailbox[$i] = $this->__mailbox[$i+1];
						unset($this->__mailbox[$i+1]);
			}				
			return $object;
		}	
			
		function lock()
		{
			$this->__lock = true;
		}
		
		function unlock()
		{
			$this->__lock = false;
		}
		
		function isLocked()
		{
			return $this->__lock;
		}
		
		function ondrop()
		{
			return;	
		}
		
	}
