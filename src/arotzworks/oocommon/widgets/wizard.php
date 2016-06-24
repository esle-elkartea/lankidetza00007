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
    
    $Id: wizard.php,v 1.5 2006/01/30 19:51:21 kilburn Exp $
*/    
        class wizard_page extends oowidget
        {
                var $type;
                var $nextpage;
                var $title;
                var $text;
                var $widgets;
                var $outpage;
		var $cancelpage;
                var $isfinish = false;
                var $__nextbutton = "Siguiente";
                var $__previousbutton = "Anterior";
                var $__cancelbutton = "Cancelar";
                var $__finishbutton = "Finalizar";

                function wizard_page ($title,$text,$type)
                {
                        $this->title =$title;
                        $this->text = $text;
                        $this->type = $type;
                }

                function addWidget($widget)
                {
                        $this->widgets[] = $widget;
                }

                function draw()
                {
                        if ($this->type=0)
                        {
                        $out ="
                        <div class=\"wizard_body\">
                        <div class=\"wizard_leftbody\">
                                <img src=\"icons/wizard.png\">
                        </div>
                        <div class=\"wizard_rightbody\">
                        ";
                        } else {
                        $out ="
                        <div class=\"wizard_body\">
                        <div class=\"wizard_topbody\">
                        <h1>".$this->title."</h1>
                        </div>
                        <div class=\"wizard_innerbody\">
                        <span>".$this->text."</span>
                        <table>
                        ";
                        }
                        for ($i=0;$i<count($this->widgets);$i++)
                        {
                                $out .=$this->widgets[$i]->draw();
                        }
                        $r = new request();
                        $out .="</table></div>
                        <div class=\"wizard_buttons\">
                        ";
                        if ($this->isfinish)
                        { 
                                $out .=" <button name=\"finish\" value=\"1\" onclick=\"javascript:document.location='".$this->outpage."';return false;\">".$this->__finishbutton."</button> ";
                        } else {
                                $out .=($r->step > 0 ? "<button name=\"step\" value=\"".($r->step-1)."\">".$this->__previousbutton."</button>":"")."<button name=\"next\" value=\"1\">".$this->__nextbutton."</button> <button name=\"cancel\" value=\"1\" 
onclick=\"javascript:document.location='".$this->cancelpage."';return false;\">".$this->__cancelbutton."</button>";
                        }
                        $out .="</div>";
                        return $out;
                }
        }


        class wizard extends oowidget
        {
                var $pages;
                var $callbackobject;
                var $icon;
                var $title;
                var $controller;
                var $items;

                function wizard($title,$controller,$callbackobject,$outpage="",$cancelpage="./")
                {
                        $this->title = $title;
                        $this->callbackobject = $callbackobject;
                        $this->controller = $controller;
                        $this->outpage = $outpage;
			$this->cancelpage = $cancelpage;
                }

                function addPage($id,$page)
                {
                        $this->pages[$id]= $page;
                        $this->pages[$id]->outpage = $this->outpage;
			$this->pages[$id]->cancelpage = $this->cancelpage;
                }
              
                function draw()
                {
                        $r = new request();
                        if ($r->cancel)
                        {
                        	return "";
                        }
                        $object = new $this->callbackobject;
                        if ($r->step == "")  
                        { 
                                $r->step = 1;  
                                $this->step = 1;
                        } else {
                                $step = $object->parsePage($r->step); // El proximo paso viene dado por el objeto
                                $this->step = $step;
                        }
                        $out ="<form method=\"post\" action=\"".$this->controller."\">
                                <input type=\"hidden\" name=\"step\" value=\"".($this->step+1)."\">";
                        foreach ($r as $k=> $v)
                        {
                                if (strpos($k,"__") === false && $k != "step" && $k != "next")
                                {
                                        $out .="<input type=\"hidden\" name=\"".$k."\" value=\"".$v."\">";
                                }
                        }
                        if (is_Object($this->pages[$this->step]))
                        $out .=$this->pages[$this->step]->draw();
                        if ($this->pages[$r->step]->isfinish)
                        {
                                $object = new $this->callbackobject;
                                $object->drop(new request());
                        }
                        return $out;
                }

        }
?>
