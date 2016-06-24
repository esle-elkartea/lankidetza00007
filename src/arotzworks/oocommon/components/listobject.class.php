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
    
    $Id: listobject.class.php,v 1.2 2005/12/27 16:25:15 luis Exp $
*/    
	class listobject extends oocomponent
	{
		function getList()
		{
			for ($i=0;$i<count($this->items);$i++)
			{
				foreach ($this->items[$i] as $k=>$v)
				{
					if (strpos($k,"__") == true)
						unset ($this->items[$i][$k]);	
				}	
			}
			return $this->items;	
		}
	}