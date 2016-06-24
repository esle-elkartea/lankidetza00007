<?php
	class puertas_paso extends oowidget
	{

		function puertas_paso($edit=true) { $this->edit = $edit;}
		function draw()
		{
			ob_start();
			if ($this->edit){
				include_once ("puertas_paso.html");
			} else {
				include_once ("puertas_paso_noedit.html");
			}
			$data = ob_get_contents();
			ob_end_clean();
			return $data;
		}		
	}
	
	class ventanas extends oowidget
	{

		function ventanas($edit=true) { $this->edit = $edit;}
		function draw()
		{
			ob_start();
			if ($this->edit){
				include_once ("ventanas.html");
			} else {
				include_once ("ventanas_noedit.html");
			}
			$data = ob_get_contents();
			ob_end_clean();
			return $data;
		}		
	}
	
	class puertas_entrada extends oowidget
	{
		function puertas_entrada($edit=true) { $this->edit = $edit;}
		function draw()
		{
			ob_start();
			if ($this->edit){
				include_once ("puertas_entrada.html");
			} else {
				include_once ("puertas_entrada_noedit.html");
			}
			$data = ob_get_contents();
			ob_end_clean();
			return $data;
		}		
	}
	
	class estadowidget extends oowidget
	{
		function draw()
		{
			$o = $this->popMailBox();
			return "<center><img src=\"estados/".($o->estado ? $o->estado : "0").".png\" /></center>";
		}
	}
	
	class estadolistwidget extends oowidget
	{
	function draw()
		{
			$o = $this->popMailBox();
			if ($o->estado == 3 || $o->estado == 5 || $o->estado == 7 || $o->estado == ""){
				$out .="<span style=\"font-size:12px;color:red\">";
			} else {
				$out .="<span style=\"font-size:12px;color:green\">";
			}
			switch ($o->estado){
				case "0": $out .= "<img src=\"icons/16x16/new.png\" style=\"vertical-align:middle;padding-right:4px;\" />Sin procesar"; break;
				case "1": $out .= "<img src=\"icons/16x16/page.png\" style=\"vertical-align:middle;padding-right:4px;\" />Orden"; break;
				case "2": $out .= "<img src=\"icons/16x16/arrow_switch.png\" style=\"vertical-align:middle;padding-right:4px;\" />Taller"; break;
				case "3": $out .= "<img src=\"icons/16x16/error.png\" style=\"vertical-align:middle;padding-right:4px;\" />Excepci&oacute;n en Taller"; break;
				case "4": $out .= "<img src=\"icons/16x16/wrench.png\" style=\"vertical-align:middle;padding-right:4px;\" />Fabricaci&oacute;n"; break;
				case "5": $out .= "<img src=\"icons/16x16/error.png\" style=\"vertical-align:middle;padding-right:4px;\" />Excepci&oacute;n en Fabricaci&oacute;n"; break;
				case "6": $out .= "<img src=\"icons/16x16/package.png\" style=\"vertical-align:middle;padding-right:4px;\" />Almac&eacute;n"; break;
				case "7": $out .= "<img src=\"icons/16x16/error.png\" style=\"vertical-align:middle;padding-right:4px;\" />Excepci&oacute;n en Almac&eacute;n"; break;
				case "8": $out .= "<img src=\"icons/16x16/car.png\" style=\"vertical-align:middle;padding-right:4px;\" />Entrega"; break;
				case "9": $out .= "<img src=\"icons/16x16/error.png\" style=\"vertical-align:middle;padding-right:4px;\" />Excepci&oacute;n en Entrega"; break;
				default: $out .="<img src=\"icons/16x16/error.png\" style=\"vertical-align:middle;padding-right:4px;\" />Excepci&oacute;n general - Error de Procesamiento";break;
			}
			$out .="</span>";
			return $out;
		}
	}

	
	/*
	 * 	Estados
	 *  
	 *  1 Orden
	 *  2 Taller
	 *  3 Excepci—n Taller
	 *  4 Fabricaci—n
	 *  5 Excepci—n Fabricaci—n
	 *  6 Almacen
	 *  7 Excepci—n Almacen
	 *  8 Entrega
	 *  9 Excepci—n Entrega
	 * 
	 */	
	class eventviewer extends oowidget
	{
		function draw()
		{
			$o = $this->popMailBox();
			$id = $o->id;
			$es= new estadowidget();
			$es->drop($o);
			$out .=$es->draw();
			$sf = new scaffold("pedidos_eventos");
			$sf->__query = "SELECT * FROM pedidos_eventos WHERE idpedido=".$id." ORDER BY fecha DESC";
			$out .="<div style=\"overflow:auto;border:inset 1px;background:white;height:250px;\">";
			$out .="<table style=\"font-size:12px;\" class=\"eventos\"><thead><td>Fecha</td><td>Estado Antiguo</td><td>Estado Nuevo</td><td>Fecha Prevista</td><td>Descripci&oacute;n</td></thead>";
			foreach ($sf->getList() as $evento)
			{
					$d = new date();
					$d->timestamp = $evento["fecha"];
					$d->parse();
					$d2 = new date();
					$d2->timestamp = $evento["fecha_entrega_prevista"];
					$d2->parse();
					$ewa =new estadolistwidget();
					$ewn =new estadolistwidget();
					$o = new oobject();
					$o->estado = $evento["estado_viejo"];
					$ewa->drop($o);
					$o = new oobject();
					$o->estado = $evento["estado_nuevo"];
					$ewn->drop($o);
					$out .="<tr><td style=\"width:110px\">".$d->string."</td><td style=\"width:158px;\">".$ewa->draw()."</td><td  style=\"width:158px;\">".$ewn->draw()."</td><td  style=\"width:110px;\">".$d2->short_string."</td><td style=\"font-size:9px;\">".$evento["descripcion"]."</td></tr>";
			}
			$out .="</table>";
			
			$out .="</div>";
			
			return $out;
		}
		
	}

class tipowidget extends oowidget
	{
	function draw()
		{
			$o = $this->popMailBox();
			switch ($o->tipo_pedido){
				case "0": $out .= "Puertas";break;
				case "1": $out .= "Ventanas";break;
				case "2": $out .= "Puertas+Ventanas";break;

			}
			$out .="</span>";
			return $out;
		}
	}
	
	
?>