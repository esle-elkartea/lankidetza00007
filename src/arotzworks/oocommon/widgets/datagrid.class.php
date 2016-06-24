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
    
    $Id: datagrid.class.php,v 1.6 2006/01/25 20:04:19 luis Exp $
*/    

	class datagrid extends oowidget
	{
		var $__headers;
		var $__hidden;
		var $__target;
		var $__key;
                var $name;
		var $__css_header = "datagrid_header";
		var $__css_field = "datagrid_field";
		var $__css_keyfield = "datagrid_keyfield";
		var $__css_table = "datagrid_table";
		var $__css_mouseover = "datagrid_row_mouseover";
		var $__css_mouseout = "datagrid_row_mouseout";
		var $__css_options = "datagrid_options";
		var $slug=false;
		var $toolbar=true;
		var $__fieldwidgets;
		
		function datagrid($key,$target,$slug=false)
		{
                        $this->name = $key;
			$this->__key = $key;
			$this->__target = $target;
			$this->slug = $slug;
		}
		
		function specialField($field,$entityfield,$entity,$widget)
		{
			$this->__fieldwidgets[$field]["entity"] =$entity;
			$this->__fieldwidgets[$field]["entityfield"] =$entityfield;	
			$this->__fieldwidgets[$field]["widget"] =$widget;
			
		}
		
		function objectField($field,$object,$property)
		{
			$this->__fieldobjects[$field]["object"] = $object;	
			$this->__fieldobjects[$field]["property"] = $property;
			
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
			$r = new request();
			if (!$object) return;
			$filter = $this->name."_filter";
                        $filter_text = $filter."_text";
                        $filter_field = $filter."_field";
                        
			$out .="<script src=\"oocommon/helpers/sorttable.js\" /></script>";
			if ($r->$filter_text!= "")
			{
				if ($r->filter_match == 0)
				{
						$object->__query = "SELECT * FROM ".$object->__table." WHERE ".$r->$filter_field." LIKE '%".$r->$filter_text."%'";
						
				} else {
						$object->__query = "SELECT * FROM ".$object->__table." WHERE ".$r->$filter_field." ='".$r->$filter_text."'";
				}
				$rows = $object->getList(); 

			} else {
			$rows = $object->getList();
			}
			if (strpos($this->target,"?") != false)
			{
				$prepend = "?datagrid=1";
			}
			if ($this->toolbar)
			{
				if ($r->datagrid_window < 1) $r->datagrid_window = 1;
				$heout ="<table class=\"".$this->__css_options."\"><tr><td class=\"".$this->__css_options."\">
				".($r->datagrid_window > 1 ? "<button onclick=\"javascript:document.location='".$_SERVER["PHP_SELF"]."?datagrid_window=".($r->datagrid_window -1).($r->$filter_text != "" ? "&".$filter_text."=".$r->$filter_text."&".$filter_field."=".$r->$filter_field : "")."'\"><img src=\"icons/16x16/actions/1leftarrow.png\">Anterior</button>" : "")."
 				".($r->datagrid_window * 20 < count($rows) ? "<button onclick=\"javascript:document.location='".$_SERVER["PHP_SELF"]."?datagrid_window=".($r->datagrid_window +1).($r->$filter_text != "" ? "&".$filter_text."=".$r->$filter_text."&".$filter_field."=".$r->$filter_field : "")."'\">Siguiente <img src=\"icons/16x16/actions/1rightarrow.png\"></button>":"")."
				</td><td class=\"".$this->__css_options."\"> ".getClassConfig($this,"message_results")." <b>".($r->datagrid_window * 20 - 20)."</b> ".getClassConfig($this,"message_to")." <b>".(($r->datagrid_window * 20) > count($rows) ? count($rows) : $r->datagrid_window * 20) ." <b></b>  ".getClassConfig($this,"message_of")."  <b>".count($rows)."</b> </td><td class=\"".$this->__css_options."\">";
			
				$heout.= count($rows)." ".getClassConfig($this,"message_results");
				$heout .="</td><td class=\"".$this->__css_options."\"><form method=\"post\" action=\"".$_SERVER["PHP_SELF"]."?datagrid=1&filter=1\"> ";
				$selector = getClassConfig($this,"message_filter")."<select name=\"".$this->name."_filter_field\">";
				$fields = $object->getFields();
				for ($i=0;$i<count($fields);$i++)
					if (strpos($fields[$i],"id") === false && $this->__hidden[$fields[$i]] == "") $selector .="<option ".($r->$filter_field == $fields[$i] ? "selected" : "")." value=\"".$fields[$i]."\">".($this->__headers[$fields[$i]] ? $this->__headers[$fields[$i]] : $fields[$i])."</option>";
				$selector .="</select>";
				$selector_input ="<input type=\"text\" name=\"".$this->name."_filter_text\" value=\"".$r->$filter_text."\"><input type=\"submit\" value=\"".getClassConfig($this,"message_filter")."\">";
				$heout .=$selector . $selector_input." ".($r->$filter_text ? "<b>FILTRO ACTIVO</b>" : "" )."</form>";
				$heout .="</td></tr></table>";
				$out .=$heout;
			}

			$tablename=md5(uniqid(rand()));
			$out .="\n<table id=\"".$tablename."\" class=\"sortable\" >\n\t<tr>\n";
			if(is_array($rows[0]))
			foreach ($rows[0] as $k=>$v)
			{
				if ($k != "id" && !$this->__hidden[$k] ) $out .="\t\t<td id=\"".$k."\" onclick=\"javascript:sort('".$tablename."','".$k."');\" class=\"".$this->__css_header."\">".($this->__headers[$k] ? $this->__headers[$k] : $k)."</td>\n";
			}
			$out .="\t</tr>\n";
			if (count($rows > 20) || $r->datagrid_window < 1)
			{
				if ($r->datagrid_window != "")
				{
					$start = 20*$r->datagrid_window-20;
					$end = 	22*$r->datagrid_window;
				} else {
					$start = 0;
					$end = count($rows);
				}
			} else {
				$start = 0;
				$end = count($rows);
			}
			// CAMBIO LAGUN 
			for ($i=0;$i<count($rows);$i++)
			{
				$out.="\t<tr class=\"".$this->__css_mouseout.($i%2==0 ? "_odd":"_even")."\" onmouseover=\"javascript:this.className='".$this->__css_mouseover."'\" onmouseout=\"javascript:this.className='".$this->__css_mouseout.($i%2==0 ? "_odd":"_even")."'\" onClick=\"javascript:document.location='".$this->__target.($this->slug ? "/".$rows[$i]["id"] : $prepend."".$this->name."_id=".$rows[$i]["id"]).($r->$filter_text != "" ? "&".$filter_text."=".$r->$filter_text."&".$filter_field."=".$r->$filter_field.($r->filter_match ? "&filter_match=1":""):"")."';\">\n";
				if ($rows[$i] != "")
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
						} else if(is_array($this->__fieldobjects[$k]) && $v !="") {
							$o = $this->__fieldobjects[$k]["object"];
							
							if(is_object($o))
							{
							$o->open($v);
							$key = $this->__fieldobjects[$k]["property"];
							
							$out .="<td class=\"".$this->__css_field."\">".$o->$key."</td>";
							}
						} else {
							if ($k != $this->__key && $k != "id")
							{
							$out .="\t\t<td class=\"".$this->__css_field."\">".$v."&nbsp;</td>\n";
							} else if ($k != "id")
							{
							$out .="\t\t<td class=\"".$this->__css_field."\"><a href=\"".$this->__target.$prepend."&".$this->name."_id=".$rows[$i]["id"].($r->$filter_text != "" ? "&".$filter_text."=".$r->$filter_text."&".$filter_field."=".$r->$filter_field.($r->filter_match ? "&filter_match=1":""):"")."\">".$v."</a></td>\n";
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
