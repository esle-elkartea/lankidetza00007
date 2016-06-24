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
    
    $Id: button.class.php,v 1.3 2006/01/30 17:14:19 luis Exp $
*/    
	class button extends oowidget
	{
		var $css;
		var $image;
		var $caption;
		var $accesskey;
		var $onclick;
		var $onclicklocation;
		
		function button($caption="",$onclick="",$image="",$accesskey="")
		{
			$this->accesskey = $accesskey;
			$this->caption = $caption;
			if ($image)
				$this->image = "icons/16x16/".$image.".png";
			if (strpos($onclick,"javascript") === false){
				$this->onclicklocation = $onclick;	} else {
						$this->onclick = $onclick;
			}
		}
		
		function drawWidget()
		{
			$o = $this->popMailBox();
			if ($this->accesskey)
			{
				$this->caption = str_replace($this->accesskey , "<u>".$this->accesskey."</u>",$this->caption);
			}
			if (is_object($o))
			{
				 return "<button ".($this->accesskey ? "accesskey=\"".$this->accesskey."\"" : "")." class=\"".$this->css."\" onclick=\"".($this->onclicklocation ? "javascript:document.location='".$this->onclicklocation."'" : $this->onclick)."\"><center>".($o->image ? "<img src=\"".$o->image."\" /> " : "").$o->caption."</center></button>";
			} else {
				 return "<button ".($this->accesskey ? "accesskey=\"".$this->accesskey."\"" : "")." class=\"".$this->css."\" onclick=\"".($this->onclicklocation ? "javascript:document.location='".$this->onclicklocation."'" : $this->onclick)."\"><center> ".($this->image ? "<img src=\"".$this->image."\" /> " : "").$this->caption."</center></button>"; 	
			}
		}
	}
?>