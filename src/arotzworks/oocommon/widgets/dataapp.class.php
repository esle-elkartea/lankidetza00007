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
    
    $Id: dataapp.class.php,v 1.6 2006/01/30 18:24:31 kilburn Exp $
*/    
class dataapp extends oowidget {
        var $message_insert_ok = "Object inserted successfully";
	var $message_insert_failed = "Insertion of object failed";
	var $message_update_ok = "Object updated successfully";
	var $message_update_failed = "Update of object failed";
	var $message_delete_ok = "Object deleted sucessfully";
	var $message_delete_confirm = "Are you sure you want to delete the object?";
	var $message_delete_failed = "Deletion of object failed";
	var $button_add = "Add Object";
	var $button_delete = "Delete Object";
	var $button_update = "Update Object";
	var $button_list = "Return to List";
	var $name;
	var $controller;
	var $table;
	var $__gridobject;
	var $__formobject;
	var $__scaffoldobject;
	var $__composites;
	var $__additionalwidgets;

	function dataapp($table, $controller, $mastercolumn) {
                $this->name = $table;
                $identificator =  $table."_id";
		$this->controller = $controller;
                $action = $this->name."_action";
		$this->message_insert_ok = getClassConfig($this, "message_insert_ok");
		$this->message_insert_failed = getClassConfig($this, "message_insert_failed");
		$this->message_update_ok = getClassConfig($this, "message_update_ok");
		$this->message_update_failed = getClassConfig($this, "message_update_failed");
		$this->message_delete_ok = getClassConfig($this, "message_delete_ok");
		$this->message_delete_failed = getClassConfig($this, "message_delete_failed");
		$this->message_delete_confirm = getClassConfig($this, "message_delete_confirm");
		$this->button_add = getClassConfig($this, "button_add");
		$this->button_update = getClassConfig($this, "button_update");
		$this->button_delete = getClassConfig($this, "button_delete");
		$this->button_list = getClassConfig($this, "button_list");
		$this->__scaffoldobject = new scaffold($table);
		$this->__gridobject = new datagrid($mastercolumn, $controller.(strpos($controller,"?") === false ? "?" : "&").$action."=view&");
                $this->__gridobject->name = $this->name;
               
                $this->__scaffoldobject->id = $r->$identificator;
		if (file_exists("ui/".$table.".ui")) {
			$this->__formobject = new uiform($table.".ui", $controller);
		} else {
			$this->__formobject = new dataform($this->controller);
		}
                $this->__formobject->name = $this->name;
	}

	function headers($orig, $name) {
		$this->__formobject->headers($orig, $name);
		$this->__gridobject->headers($orig, $name);
	}

	function disablefield($field) {
		$this->__formobject->disable($field);
	}
	function hidefield($field) {
		$this->__gridobject->hidefield($field);
		$this->__formobject->hidefield($field);
	}

	function composeField($field, $entity, $efield) {
		$this->__composites[$field]["entity"] = $entity;
		$this->__composites[$field]["efield"] = $efield;
	}

	function specialField($localfield, $entityfield, $entity, $widget) {
		$this->__gridobject->specialField($localfield, $entityfield, $entity, $widget);
		$this->__formobject->specialField($localfield, $entityfield, $entity, $widget);
	}

	function objectField($field, $object, $property) {
		$this->__gridobject->objectField($field, $object, $property);
		$this->__formobject->objectField($field, $object, $property);
	}

	function addWidget($widget) {
		$this->__additionalwidgets[] = $widget;
	}

	function predrawWidget() {
        $identificator = $this->name;
        $action = $this->name."_action";
        $_identificator = $this->name."_id";
		$r = new request();
		if ($r->filter_text)
		{
			if (strpos($this->controller,"?") === false)
			{
				$this->controller .="?prepend=1";	
			} 
			$this->controller .="&filter_text=".$r->filter_text."&filter_field=".$r->filter_field;
			if ($r->filter_match)
			{
				$this->controller .="&filter_match=1";	
			}	
		}
               
		if ($r->$_identificator)
			$this->__scaffoldobject->open($r->$_identificator);
                
		$this->__gridobject->drop($this->__scaffoldobject);
		if ($r->$action == "insert")
		{
			$this->__scaffoldobject->fromObject($r);
		}
		$this->__formobject->drop($this->__scaffoldobject);

		switch ($r->$action) {
			default :

				$button_add = new button($this->button_add, $this->controller.(strpos($this->controller,"?") === false ? "?" : "&").$action."=add","add");
				$b = new box($this->name ? $this->name : "List", $this->__gridobject);
				if (!$this->__disable_add) $b->addWidget($button_add);
				return $b;
				break;

			case "add" :
				$this->__formobject->action = $this->__formobject->action.(strpos($this->__formobject->action,"?") === false ? "?" : "&").$action."=insert";
				$this->__formobject->submit = $this->button_add;
				$this->__formobject->__target = $this->__formobject->action;
				return new box($this->name ? $this->name : "Add", $this->__formobject);
				break;

			case "insert" :
				$this->__scaffoldobject->fromObject($r);
				if (is_array($this->__composites))
				foreach ($this->__composites as $k => $v) {
					$entity = $this->__composites[$k]["entity"];
					$efield = $this->__composites[$k]["efield"];
					$this->__scaffoldobject->composeField($k, $entity, $efield);
				}
				if ($this->__scaffoldobject->insert()) {
					$b = new box($this->name ? $this->name : "Message", html :: write($this->message_insert_ok));
					$t = new toolbar();
					$b->addWidget(html::write("<script>setTimeout(\"document.location='".$this->controller."'\",1000);</script>"));
					
					
					$b->addWidget($t);
					return $b;
				} else {
					$w = new oowidget();
					$b = new box($this->name ? $this->name : "Message", html :: write($this->message_insert_failed));
                                        $b->addWidget(errorobject :: errorList());
                                        $this->__formobject->action = $this->__formobject->action.(strpos($this->__formobject->action,"?") === false ? "?" : "&").$action."=insert";
					$this->__formobject->submit = $this->button_add;
					$this->__formobject->__target = $this->__formobject->action;
				        $b2 = new box($this->name ? $this->name : "Add", $this->__formobject);
					$w->addWidget($b);
					$w->addWidget($b2);
					$b->addWidget(new button($this->button_list, $this->controller));
					return $w;
				}
				break;

			case "update" :
                                $tmpsf = $this->__scaffoldobject;
				$this->__scaffoldobject = new scaffold($this->name);
				$this->__scaffoldobject->__validators = $tmpsf->__validators;
				$this->__scaffoldobject->fromObject($r);
				if (is_array($this->__composites))
				foreach ($this->__composites as $k => $v) {
					$entity = $this->__composites[$k]["entity"];
					$efield = $this->__composites[$k]["efield"];
					$this->__scaffoldobject->composeField($k, $entity, $efield);
				}
				
				if ($this->__scaffoldobject->update()) {
					$b = new box($this->name ? $this->name : "Message", html :: write($this->message_update_ok));
					$t = new toolbar();
					//$b->addWidget(new button($this->button_list, $this->controller));
					$b->addWidget(html::write("<script>setTimeout(\"document.location='".$this->controller."'\",1000);</script>"));
					return $b;
				} else {
					$w = new oowidget();
					$this->__formobject->action = $this->__formobject->action.(strpos($this->__formobject->action,"?") === false ? "?" : "&").$action."=update&".$this->name."_id=".$r->$_identificator;
					$this->__formobject->__target = $this->__formobject->action;
					$this->__formobject->submit = $this->button_update;
					$b2 = new box($this->name ? $this->name : "View", $this->__formobject);
					
					$b = new box($this->name ? $this->name : "Message", html :: write($this->message_update_failed));
					$b->addWidget(errorobject :: errorList());
                                        $b->addWidget(new button($this->button_list, $this->controller));
                                        
                                        $w->addWidget($b);
					$w->addWidget($b2);
					$b->addWidget(new button($this->button_list, $this->controller));
					return $w;
					return $b;
				}
				break;

			case "view" :
				$this->__formobject->action = $this->__formobject->action.(strpos($this->__formobject->action,"?") === false ? "?" : "&").$action."=update&".$this->name."_id=".$r->$_identificator;
				$this->__formobject->__target = $this->__formobject->action;
				$this->__formobject->submit = $this->button_update;
				$b = new box($this->name ? $this->name : "View", $this->__formobject);
				for ($i = 0; $i < count($this->__additionalwidgets); $i ++) {
					$b->addWidget($this->__additionalwidgets[$i]);
				}
				$t = new toolbar();
				$t->addWidget(new button($this->button_list, $this->controller,"book_previous"));
				$t->addWidget(new button($this->button_delete, $this->controller.(strpos($this->controller,"?") === false ? "?" : "&").$action."=delete&".$_identificator."=".$r->$_identificator,"delete"));
				$b->addWidget($t);
				return $b;
				break;

                        case "delete":
                                $b = new box($this->name ? $this->name : "Delete");
                                $b->addWidget(html::write($this->message_delete_confirm));
                                $t = new toolbar();
                                $t->addWidget(new button($this->button_delete, $this->controller.(strpos($this->controller,"?") === false ? "?" : "&").$action."=delete_confirm&".$_identificator."=".$r->$_identificator,"delete"));
                                $t->addWidget(new button($this->button_list, $this->controller.(strpos($this->controller,"?") === false ? "?" : "&").$action."=&".$_identificator."=".$r->$_identificator,"book_previous"));
                                $b->addWidget($t);
                                return $b;
                                break;

			case "delete_confirm" :
				$this->__scaffoldobject->open($r->$_identificator);
				if ($this->__scaffoldobject->delete()) {
					$b = new box($this->name ? $this->name : "Message", html :: write($this->message_delete_ok));
					$t = new toolbar();
					$t->addWidget(new button($this->button_list, $this->controller));
					$t->addWidget(html::write("<meta http-equiv=\"refresh\" content=\"1; url=".$this->controller."\" />"));
					$b->addWidget($t);
					return $b;
				} else {
					$b = new box($this->name ? $this->name : "Message", html :: write($this->message_delete_failed));
					$b->addWidget(errorobject :: errorList());
					$b->addWidget(new button($this->button_list, $this->controller));
					return $b;
				}
				break;
		}
	}

	function draw() {
		$o = $this->preDrawWidget();
		return $o->draw();
	}
}
