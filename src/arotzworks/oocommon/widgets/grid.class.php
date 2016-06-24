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
    
    $Id: grid.class.php,v 1.2 2005/12/27 16:25:15 luis Exp $
*/    

	class grid extends oowidget
	{
		var $__headers;
		var $__hidden;
		var $__target;
		var $__key;
		var $__css_header = "grid_header";
		var $__css_field = "grid_field";
		var $__css_keyfield = "grid_keyfield";
		var $__css_table = "grid_table";
		var $arrayfield ="items";
		function grid($key,$target)
		{
			$this->__key = $key;
			$this->__target = $target;
		}
		
		function specialField($field,$entityfield,$entity,$widget)
		{
			$this->__fieldwidgets[$field]["entity"] =$entity;
			$this->__fieldwidgets[$field]["entityfield"] =$entityfield;	
			$this->__fieldwidgets[$field]["widget"] =$widget;
			
		}
		
		function headers($original,$translation)
		{
			$this->__headers[$original] = $translation;
		}
		
		function hidefield($field)
		{
			$this->__hidden[$field] = true;	
		}
		
		function drawWidget()
		{
			$object = $this->popMailbox();
			
			if (!$object) return;
			$rows = $object->items;
			if (strpos($this->target,"?") != false)
			{
				$prepend = "?datagrid=1";
			}
			if (!$rows)
			{
				debug($this,"Dataobject returned no rows");
				return "&nbsp;";
			}
			$out .="\n<table class=\"".$this->__css_table."\" style=\"width:100%\">\n\t<tr>\n";
			foreach ($rows[0] as $k=>$v)
			{
				if ($k != "id" && !$this->__hidden[$k] ) $out .="\t\t<td class=\"".$this->__css_header."\">".($this->__headers[$k] ? $this->__headers[$k] : $k)."</td>\n";
				
			}
			$out .="\t</tr>\n";
			
			for ($i=0;$i<count($rows);$i++)
			{
				$out.="\t<tr>\n";
				foreach($rows[$i] as $k=>$v)
				{
					unset($w);
					if (!$this->__hidden[$k])
					{
						if (is_array($this->__fieldwidgets[$k]))
						{
							if (is_object($this->__fieldwidgets[$k]["entity"])===false) 
							{
								$ef = $this->__fieldwidgets[$k]["entityfield"];
								$o = new $this->__fieldwidgets[$k]["entity"]();
								$o->$ef = $v;
								$w = new $this->__fieldwidgets[$k]["widget"]();
								$w->drop($o);
							} else {
								
								$o = $this->__fieldwidgets[$k]["entity"];
								$do = new dataobject();
								foreach ($rows[$i] as $w =>$s)
								{
									$do->$w = $s;
								}
								$o->drop($do);
								$w = new $this->__fieldwidgets[$k]["widget"]();
								$w->drop($o);
							}
							
							$out .="<td class=\"".$this->__css_field."\">".$w->draw()."</td>";
						
						} else {
							if ($k != $this->__key && $k != "id")
							{
							$out .="\t\t<td class=\"".$this->__css_field."\">".$v."&nbsp;</td>\n";
							} else if ($k != "id")
							{
							$out .="\t\t<td class=\"".$this->__css_field."\"><a href=\"".$this->__target.$prepend."&id=".$rows[$i]["id"]."\">".$v."</a></td>\n";
							}
						} 
					}
				}
				$out.="\t</tr>\n";	
			}
			$out .="</table>";
			return $out;				
		}	
		
		
	}
