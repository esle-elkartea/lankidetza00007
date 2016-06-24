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
    
    $Id: form.class.php,v 1.4 2006/01/30 17:14:19 luis Exp $
*/    

	$fsecuence = 0;
        class form_input extends oowidget
        {
                var $name;
                var $caption;
                var $value;
                var $description;
                var $size;
                var $type = "text";
                var $enabled=true;
                function form_input($name,$caption,$value="",$description="",$size="20",$type="text",$enabled=true)
                {
                    $this->name = $name;
                    $this->caption = $caption;
                    $this->value = $value;
                    $this->description = $description;
                    $this->size = $size;
                    $this->type=$type;
                    $this->enabled = $enabled;
                }

                function draw()
                {       
		   
		    $errors = errorobject::errorParams();
		    global $fsecuence;
                    return "<tr class=\"form_row".($fsecuence%2==0 ? "_odd":"_even")."\"><td class=\"form_row_caption\"><span class=\"form_caption\" ".($errors[$this->name] ? "style=\"color:red\"" : "" ).">".$this->caption."</span><br /><span class=\"form_caption_subcaption\">".$this->description."</span></td><td class=\"form_row_field\"><input  ".($errors[$this->name] ? "style=\"border: solid 2px red;margin:-2px;\"" : "" )." type=\"".$this->type."\" value=\"".$this->value."\" name=\"".$this->name."\" size=\"".$this->size."\" ".($this->enabled ? "" : "readonly=\"readonly\"")." /></td></tr>";
                }
        }
        class form_hidden extends oowidget
        {
                var $name;
                var $value;
                var $enabled=true;
                function form_hidden($name,$value="")
                {
                    $this->name = $name;
                    $this->value = $value;
                }

                function draw()
                {       
                    return "<input type=\"hidden\" value=\"".$this->value."\" name=\"".$this->name."\" size=\"".$this->size."\" ".($this->enabled ? "" : "readonly=\"readonly\"")." />";
                }
        }

        class form_unique extends oowidget
        {
                var $name;
                var $caption;
                var $description;
                var $options;
                
                function form_unique($name,$caption,$description="")
                {
                    $this->name = $name;
                    $this->caption = $caption;
                    $this->description = $description;
                }
                
                function addOptions($list)
                {
                        $this->options = $list;
                }

                function draw()
                {
                        global $fsecuence;
                   	$out ="<tr class=\"form_row".($fsecuence%2==0 ? "_odd":"_even")."\"><td class=\"form_row_caption\"><span class=\"form_caption\">".$this->caption."</span><br />".$this->description."</td></tr><tr><td class=\"form_row_field\">
                        ";

                        for ($i=0;$i<count($this->options);$i++)
                        {
                        $out .= " <span class=\"form_caption\"><input type=\"radio\" name=\"".$this->name."\" value=\"".$this->options[$i]["value"]."\">".$this->options[$i]["caption"]."</span>
                       ".$this->options[$i]["description"];        
                        }
                        $out.="</td></tr>";
                        return $out;
                }
        }

	class form_checkbox extends oowidget {
	
	  	var $name;
                var $caption;
                var $value;
                var $description;
                var $size;
                var $type = "text";
                var $enabled=true;
                function form_checkbox($name,$caption,$value="",$description="",$size="20",$type="text",$enabled=true)
                {
                    $this->name = $name;
                    $this->caption = $caption;
                    $this->value = $value;
                    $this->description = $description;
                    $this->size = $size;
                    $this->type=$type;
                    $this->enabled = $enabled;
                }

                function draw()
                {       
                    global $fsecuence;
                    return "<tr class=\"form_row".($fsecuence%2==0 ? "_odd":"_even")."\"><td class=\"form_row_caption\" colspan=\"2\"><input type=\"checkbox\" ".($this->value ? "checked" : "")." name=\"".$this->name."\" size=\"".$this->size."\" ".($this->enabled ? "" : "readonly=\"readonly\"")." /><span class=\"form_caption\">".$this->caption."</span>".$this->description."</td></tr>";
                }
	
	}

        class form_select extends oowidget
        {
                var $name;
                var $caption;
                var $description;
                var $options;

                function form_select($name,$caption,$description="")
                {
                    $this->name = $name;
                    $this->caption = $caption;
                    $this->description = $description;
                }
                
                function addOptions($list)
                {
                        $this->options = $list;
                }

                function draw()
                {
                        global $fsecuence;
                   	$out = "<tr class=\"form_row".($fsecuence%2==0 ? "_odd":"_even")."\"><td class=\"form_row_caption\"><span class=\"form_caption\">".$this->caption."</span><br />".$this->description."</td><td class=\"form_row_field\">";
                        $out .="<select name=\"".$this->name."\">";
                        for ($i=0;$i<count($this->options);$i++)
                        {
                                $out .="<option value=\"".$this->options[$i]["value"]."\" ".($this->options[$i]["selected"] == true ? "selected" : "").">".$this->options[$i]["caption"]."</option>";
                        }
                        $out .="</select></td></tr>";
                        return $out;
                }
        }
        class form_html extends oowidget
        {
                var $caption;
                function form_html($title,$caption)
                {
                    $this->title = $title;    
                    $this->caption = $caption;
                }

                function draw()
                {       
                    global $fsecuence;
                    return "<tr class=\"form_row".($fsecuence%2==0 ? "_odd":"_even")."\"><td class=\"form_row_caption\"><span class=\"form_caption\">".$this->title."</span></td><td class=\"form_row_field\" ><div>".$this->caption."</div></td></tr>";
                }

        }

        class form_textarea extends oowidget
        {
                var $name;
                var $caption;
                var $value;
                var $description;

                function form_textarea($name,$caption,$value="",$description="")
                {
                    $this->name = $name;
                    $this->caption = $caption;
                    $this->value = $value;
                    $this->description = $description;
                }

                function draw()
                {       
                   global $fsecuence;
  		$errors = errorobject::errorParams();
                    return "<tr class=\"form_row".($fsecuence%2==0 ? "_odd":"_even")."\"><td class=\"form_row_caption\"><span class=\"form_caption\"  ".($errors[$this->name] ? "style=\"color: red;\"" : "" ).">".$this->caption."</span><br />".$this->description."</td><td class=\"form_row_field\"><textarea name=\"".$this->name."\"  ".($errors[$this->name] ? "style=\"border: solid 2px red;margin:-2px;\"" : "" ).">".$this->value."</textarea></td></tr>";
                }
        }

        class form_fckeditor extends oowidget
        {
                var $name;
                var $caption;
                var $value;
                var $description;
				var $width = 600;
				var $height = 300;
                function form_textarea($name,$value)
                {
                    $this->name = $name;
                    $this->value = $value;
                   
                }

                function draw()
                {       
                   global $fsecuence;
  		$errors = errorobject::errorParams();
                   // return "<tr class=\"form_row".($fsecuence%2==0 ? "_odd":"_even")."\"><td class=\"form_row_caption\"><span class=\"form_caption\"  ".($errors[$this->name] ? "style=\"color: red;\"" : "" ).">".$this->caption."</span><br />".$this->description."</td></tr><tr><td class=\"form_row_field\"><textarea name=\"".$this->name."\"  ".($errors[$this->name] ? "style=\"border: solid 2px red;margin:-2px;\"" : "" ).">".$this->value."</textarea></td></tr>";
				 
				$oFCKeditor = new FCKeditor($this->name);
				$oFCKeditor->BasePath = '/FCKeditor/';
				$oFCKeditor->Value = $this->value;
				$oFCKeditor->Height = $this->height;
				$oFCKeditor->Width = $this->width;
				$out .= ($oFCKeditor->CreateHTML());
				  return "<tr class=\"form_row".($fsecuence%2==0 ? "_odd":"_even")."\"><td colspan=\"2\" class=\"form_row_field\">".$out."</td></tr>";	
                }
        }



        class form_separator extends oowidget
        {
                var $title;
                function form_separator($title)
                {
                        $this->title =  $title;
                }

                function draw()
                {

               global $fsecuence;
                    return "<tr class=\"form_row".($fsecuence%2==0 ? "_odd":"_even")."\"><td class=\"form_separator\" colspan=\"2\">".$this->title."</td></tr>";
                }
        }

        class form_widget extends oowidget
        {
                var $__widget;
                var $caption;
                var $description;
                function form_widget($caption,$description,$widget)
                {
                        $this->caption = $caption;
                        $this->description = $description;
                        $this->__widget = $widget;
                }

                function draw()
                {
                        assert($this->__widget);
                       global $fsecuence;
                    return "<tr class=\"form_row".($fsecuence%2==0 ? "_odd":"_even")."\"><td class=\"form_row_caption\"><span class=\"form_caption\">".$this->caption."</span><br />".$this->description."</td><td class=\"form_row_field\" colspan=\"2\">".$this->__widget->draw()."</td></tr>";
                }

        }


        class form extends oowidget
        {
            
                var $name;
                var $method = "POST";
                var $action;
                var $__components;
                var $__submit="Submit";
                var $button = true;
                function form($name,$action,$method="POST")
                {
                    $this->name=$name;
                    $this->method = $method;
                    $this->action = $action;
                    $this->__submit = getClassConfig($this,"submit_caption");
                }

                function addWidget($widget)
                {
                       $this->__components[] = $widget; 
                }


                function draw()
                {
                       if ($this->method != false)$out = "<form method=\"".$this->method."\" id=\"".$this->name."\" name=\"".$this->name."\" action=\"".$this->action."\">";
                       $out .="<table class=\"form\">";
                       for ($i=0;$i<count($this->__components);$i++)
                       {
                                assert($this->__components[$i]);
				global $fsecuence;
				$fsecuence++;
                                $out .=$this->__components[$i]->draw();
                       }
                       if ($this->button == false) $out .="<tr><td colspan=\"2\" class=\"row_submit\"><input type=\"submit\" value=\"".$this->__submit."\"></td></tr></table></form>";
                       $out .="</table>
		       <script>
		       		theform = document.getElementById('".$this->name."')	;
		       		theform.getElementsByTagName('input')[0].focus();
		       </script>
			</form>
		       "; 
                       return $out;
                }
        }
?>