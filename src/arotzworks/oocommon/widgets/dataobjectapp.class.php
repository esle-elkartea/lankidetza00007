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
    
    $Id: dataobjectapp.class.php,v 1.2 2005/12/27 16:25:15 luis Exp $
*/    
	class dataobjectapp extends oowidget
	{
		
		var $message_insert_ok = "Object inserted successfully";
		var $message_insert_failed = "Insertion of object failed";
		var $message_update_ok = "Object updated successfully";
		var $message_update_failed = "Update of object failed";
		var $message_delete_ok = "Object deleted sucessfully";
		var $message_delete_failed = "Deletion of object failed";
		var $button_add = "Add Object";
		var $button_delete = "Delete Object";
		var $button_update = "Update Object";
		var $button_list = "Return to List";
		var $name;
		var $controller;
		var $table;
		var $__gridobject;
		var $__dataform;
		var $__scaffoldobject;
		
		function dataobjectapp($object,$controller,$mastercolumn)
		{
			$this->controller = $controller;
			$this->message_insert_ok =  getClassConfig($this,"message_insert_ok");
			$this->message_insert_failed =  getClassConfig($this,"message_insert_failed");
			$this->message_update_ok =  getClassConfig($this,"message_update_ok");
			$this->message_update_failed =  getClassConfig($this,"message_update_failed");
			$this->message_delete_ok =  getClassConfig($this,"message_delete_ok");
			$this->message_delete_failed =  getClassConfig($this,"message_delete_failed");
			$this->button_add =  getClassConfig($this,"button_add");
			$this->button_update =  getClassConfig($this,"button_update");
			$this->button_delete =  getClassConfig($this,"button_delete");
			$this->button_list =  getClassConfig($this,"button_list");
			$this->__scaffoldobject = new $object;
			$this->__gridobject = new datagrid($mastercolumn,$controller."?action=view");
			if (file_exists("ui/".$table.".ui")){
				$this->__dataform = new uiform($table.".ui",$controller);
			} else {
				$this->__dataform = new dataform($this->controller);	
			}
		}	
		
		function specialField($localfield,$entityfield,$entity,$widget)
		{
			$this->__gridobject->specialField($localfield,$entityfield,$entity,$widget);
		}
		
		
		function predrawWidget()
		{
			$r = new request();
			if ($r->id)
				$this->__scaffoldobject->open($r->id);
			$this->__gridobject->drop($this->__scaffoldobject);
			$this->__dataform->drop($this->__scaffoldobject);
			
			switch ($r->action)
			{
				default:
					
					$button_add = new button($this->button_add,$this->controller."?action=add");
					$b =  new box ($this->name? $this->name : "List", $this->__gridobject);
					$b->addWidget($button_add);
					return $b;
				break;
				
				case "add":
					$this->__dataform->action = $this->__dataform->action ."?action=insert";
					$this->__dataform->submit = $this->button_add;
					$this->__dataform->__target = $this->__dataform->action;
					return new box ($this->name? $this->name : "Add",$this->__dataform);
				break;
						
					
				case "insert":
					$this->__scaffoldobject->fromObject($r);
					if ($this->__scaffoldobject->insert()){
						$b = new box($this->name  ? $this->name : "Message",html::write($this->message_insert_ok));
						$b->addWidget(new button($this->button_list,$this->controller));
						return $b;
					} else {
						$b =  new box($this->name  ? $this->name : "Message",html::write($this->message_insert_failed));
						$b->addWidget(new button($this->button_list,$this->controller));
						return $b;
					}									
				break;
				
				case "update":
					$this->__scaffoldobject->open($r->id);
					$this->__scaffoldobject->fromObject($r);
					if ($this->__scaffoldobject->update()){
						$b =  new box($this->name  ? $this->name : "Message",html::write($this->message_update_ok));
						$b->addWidget(new button($this->button_list,$this->controller));
						return $b;
					} else {
						$b = new  box($this->name  ? $this->name : "Message",html::write($this->message_update_failed));
					 	$b->addWidget(new button($this->button_list,$this->controller));
						return $b;
					}
				break;
				
				case "view":
					$this->__dataform->action = $this->__dataform->action . "?action=update&id=".$r->id;
					$this->__dataform->__target = $this->__dataform->action ;
					$this->__dataform->submit = $this->button_update;
					$b = new box($this->name  ? $this->name : "View",$this->__dataform);
					$b->addWidget(new button($this->button_list,$this->controller));
					$b->addWidget(new button($this->button_delete,$this->controller."?action=delete&id=".$r->id));
					return $b;			
				break;
				
				
				case "delete":
					$this->__scaffoldobject->open($r->id);
					if ($this->__scaffoldobject->delete()){
						$b = new box($this->name  ? $this->name : "Message",html::write($this->message_delete_ok));
						$b->addWidget(new button($this->button_list,$this->controller));
						return $b;
					} else {
						$b = new box($this->name  ? $this->name : "Message",html::write($this->message_delete_failed));
						$b->addWidget(new button($this->button_list,$this->controller));
						return $b;
					}
				break;	
				}
				
			}
			
			function draw()
			{
				$o = $this->preDrawWidget();
				return $o->draw();	
			}
		}
