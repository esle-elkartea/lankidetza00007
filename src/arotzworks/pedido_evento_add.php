<?php
include ("local.php");
$sf = new scaffold("pedidos");
$sf->open($r->pedido_id);
$p = new aworkspage();
if ($r->fecha_entrega_prevista_day)
{
	$sfe = new scaffold("pedidos_eventos");
	$sfe->fromObject($r);
	$sfe->estado_viejo = $sf->estado;
	$d = new date();
	$d->day = $r->fecha_entrega_prevista_day;
	$d->month = $r->fecha_entrega_prevista_month;
	$d->year = $r->fecha_entrega_prevista_year;
	$d->parse();
	$sfe->fecha_entrega_prevista = $d->timestamp;
	$sfe->fecha = time();
	$sfe->idpedido = $r->pedido_id;
	$sfe->insert();
	if (!errorobject::hasErrors()){
		$sf->estado = $sfe->estado_nuevo;
		$sf->fecha_entrega_prevista = $sfe->fecha_entrega_prevista;
		if ($sfe->estado_nuevo == 8)
		{
			$sf->fecha_entrega = time();
		}
		$sf->update();
		header("location:pedido.php?pedido_id=".$r->pedido_id);
	} else {
		$p->addWidget(new infobox("Error","Error en el formulario, compruebe los datos",false));
	}
}

error_reporting(E_ALL);
$p->addWidget(html::write("<h1>A&ntilde;adir evento a pedido <b>".$sf->referencia."</b> de <b>".$sf->cliente."</b> </h1>"));
$b = new Box("Datos del Evento");
$t = new toolbar();
$t->addWidget(new button("Guardar Datos del Evento","javascript:document.forms[0].submit()","disk"));
$b->addWidget($t);
$f = new form("","");

$opts[] = array("caption" => "Sin procesar", "value" => 0);
$opts[] = array("caption" => "Orden", "value" => 1);
$opts[] = array("caption" => "Taller", "value" => 2);
$opts[] = array("caption" => "Excepci&oacute;n Taller", "value" => 3);
$opts[] = array("caption" => "Fabricaci&oacute;n", "value" => 4);
$opts[] = array("caption" => "Excepci&oacute;n en Fabricaci&oacute;n", "value" => 5);
$opts[] = array("caption" => "Almac&eacute;n", "value" => 6);
$opts[] = array("caption" => "Excepci&oacute;n en Almac&eacute;n", "value" => 7);
$opts[] = array("caption" => "Entrega", "value" => 8);
$opts[] = array("caption" => "Excepci&oacute;n en Entrega", "value" => 9);
$sel = new form_select("estado_nuevo","Nuevo estado");
$sel->addOptions($opts);

$ew = new estadolistwidget();
$ew->drop($sf);
$f->addWidget(new form_widget("Estado Actual","",$ew));
$f->addWidget($sel);
$d = new date();
$d->timestamp = $sf->fecha_entrega_prevista;

$ds = new dateselector("fecha_entrega_prevista",$d->timestamp);
$ds->drop($d);

$f->addWidget(new form_widget("Nueva fecha de entrega prevista","",$ds));
$f->addWidget(new form_textarea("descripcion","Descripci&oacute;n del Evento",""));

$b->addWidget($f);
$p->addWidget($b);
$p->draw();
