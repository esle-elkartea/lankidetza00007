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
    
    $Id: dataform.class.php,v 1.4 2006/01/30 17:14:19 luis Exp $
*/    
class dataform extends oowidget {
	var $action;
	var $submit;
        var $name;
	var $css_form = "form";
	var $__hidden;
	var $__disabled;
	function dataform($action, $object = NULL, $submit = "Submit") {
		$this->action = $action;
		$this->submit = $submit;
		$this->hidefield("id");
		if ($object)
			$this->drop($object);
	}

	function hidefield($field) {
		$this->__hidden[$field] = true;
	}
	
	function disable($field) {
		$this->__disabled[$field] = true;	
	}

	function headers($original,$translation)
	{
		$this->__headers[$original] = $translation;
	}

	function specialField($field, $entityfield, $entity, $widget) {
		$this->__fieldwidgets[$field]["entity"] = $entity;
		$this->__fieldwidgets[$field]["entityfield"] = $entityfield;
		$this->__fieldwidgets[$field]["widget"] = $widget;
	}

	function objectField($field, $object, $property) {
		$this->__fieldobjects[$field]["object"] = $object;
		$this->__fieldobjects[$field]["property"] = $property;
	}

	function draw() {
		$o = $this->popMailBox();
		if (get_class($o) != "scaffold") {
			$sc = new scaffold($o->__table);
			$o->__datafields = $sc->__datafields;
		}
		$this->name = md5($this->name);                
		$out .= "<form id=\"".$this->name."\"  class=\"".$this->css_form."\" method=\"post\" action=\"".$this->action."\">";
		$out .= "<input type=\"hidden\" name=\"".$this->name."_id"."\" value=\"".$o->id."\" >";
		$out .= "<table>";
		$count=1;
		$errors = errorobject::errorParams();
		foreach ($o as $k => $v) {
			
			if (strpos($k, "__") === false) {
				if ($this->__hidden[$k] == true) {
					$out .= ($count %2 == 0 ? "<tr>" : "")."<td colspan=\"2\"><input type=\"hidden\" name=\"".$k."\" value=\"".$v."\"></td></tr>";
				} else {
					
					$out .= ($count %2 == 0 ? "<tr>" : "")."<td class=\"dataform_title\"><b>".($this->__headers[$k] ? $this->__headers[$k] : $k)."</b></td><td class=\"dataform\" style=\"padding-right:33px;\">";

					if (is_array($this->__fieldobjects[$k])) {
						$do = $this->__fieldobjects[$k]["object"];
						$list = $do->getList();

						$key = $this->__fieldobjects[$k]["property"];
						$sout = "<select  ".($errors[$k] ? "class=\"dataform_error\"" : "")." name=\"".$k."\"><option value=\"0\">---</option>";
						for ($li = 0; $li < count($list); $li ++) {
							if ($list[$li]["id"] != $v) {
								$sout .= "<option value=\"".$list[$li]["id"]."\">".$list[$li][$key]."</option>";
							} else {
								$sout .= "<option value=\"".$list[$li]["id"]."\" selected style=\"font-weight:bold\">".$list[$li][$key]."</option>";
							}

						}
						$sout .= "</select>";
						$out .= $sout."</td>".($count %2 == 0 ? "</tr>" : "")."";
						$count++;
						continue;
					}

					if (is_array($this->__fieldwidgets[$k])) {
						if (is_object($this->__fieldwidgets[$k]["entity"]) === false) {
							$ef = $this->__fieldwidgets[$k]["entityfield"];
							$eo = new $this->__fieldwidgets[$k]["entity"] ();
							$eo-> $ef = $v;
							$w = new $this->__fieldwidgets[$k]["widget"] ();
							$w->drop($eo);
						} else {

							$eo = $this->__fieldwidgets[$k]["entity"];
							$do = new dataobject();
							foreach ($rows[$i] as $w => $s) {
								$do-> $w = $s;
							}
							$eo->drop($do);
							$w = new $this->__fieldwidgets[$k]["widget"] ();
							$w->drop($eo);
						}

						$out .= $w->draw()."</td>".($count %2 == 0 ? "</tr>" : "")."";
						$count++;
						continue;
					}
					switch ($o->__datafields[$k]) {
						
						case "bool" :
							$out .= "<input ".($fc == 0 ? "id=\"first\"" :"")." ".($errors[$k] ? "class=\"dataform_error\"" : "")." type=\"checkbox\" name=\"".$k."\" ". ($v == true ? "checked" : "")." />&nbsp;</input>";
							break;
						case "int" :
							$out .= "<input  ".($fc == 0 ? "id=\"first\"" :"")." ".($errors[$k] ? "class=\"dataform_error\"" : "")." type=\"text\" ".($this->__disabled[$k] ? "readonly=\"readonly\"" : "")." name=\"".$k."\" value=\"".$v."\" size=\"10\"/>";
							break;
						case "float" :
							$out .= "<input ".($fc == 0 ? "id=\"first\"" :"")." ".($errors[$k] ? "class=\"dataform_error\"" : "")." type=\"text\" ".($this->__disabled[$k] ? "readonly=\"readonly\"" : "")." name=\"".$k."\" value=\"".$v."\" size=\"10\"/>";
							break;
						case "varchar" :
							$out .= "<input ".($fc == 0 ? "id=\"first\"" :"")."  ".($errors[$k] ? "class=\"dataform_error\"" : "")."type=\"text\" ".($this->__disabled[$k] ? "readonly=\"readonly\"" : "")." name=\"".$k."\" value=\"".$v."\" size=\"30\"/>";
							break;
						case "text" :
							$out .= "<textarea  ".($fc == 0 ? "id=\"first\"" :"")." ".($errors[$k] ? "class=\"dataform_error\"" : "")."cols=\"40\" rows=\"4\" name=\"".$k."\">".$v."</textarea>";
							break;
						$fc++; // hack para obtener foco en el primer field del formulario
					}
				}
				$count++;
				$out .= "</td>".($count %2 == 0 ? "</tr>" : "")."";
				
			}
		}
		$out .="
		<script>
		function getFocus()
		{
				f = document.getElementById('first').focus();
		}
		getFocus();
		</script>";
		$out .= "<tr><td colspan=\"2\"><button onclick=\"document.getElementById('".$this->name."').submit();\"><img src=\"icons/16x16/resultset_next.png\">".$this->submit."</button> </td></tr></table></form>";
		return $out;
	}
}
