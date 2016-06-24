<?php
include ("local.php");
error_reporting(E_ALL);
if ($r->referencia)
{
	$sf = new scaffold("pedidos");
	$sf->fromObject($r);
	$sf->estado = 0;
	$d = new date();
	$d->day = $r->entrega_prevista_day;
	$d->month = $r->entrega_prevista_month;
	$d->year = $r->entrega_prevista_year;
	$d->parse();
	$sf->fecha_entrega_prevista = $d->timestamp;
	$sf->fecha_pedido = time();
	$sf->fecha_entrega = -1;
	$tracking = "AW/".date("y").date("w")."/".strtoupper(substr(md5(uniqid(rand)),4,2).substr(md5(uniqid(rand)),7,2).substr(md5(uniqid(rand)),12,2));
	$sf->tracking = $tracking;
	$sf->insert();
	if (errorobject::haserrors())
	{
	
	} else {
		$id = mysql_insert_id();
		header ("location:pedido.php?pedido_id=".$id);
	}
}
$p = new aworkspage();
$b = new Box("A&ntilde;adir Pedido");
$p->addWidget(html::write("<h1>A&ntilde;adir Pedido</h1>"));
$t = new toolbar();
$t->addWidget(new button("Guardar Pedido","javascript:document.forms[0].submit()","add"));
$b->addWidget($t);
$f = new form("","");
$f->addWidget(new form_separator("Referencia y Cliente"));
$f->addWidget(new form_input("referencia","Referencia Cliente",$r->referencia));
$f->addWidget(new form_input("cliente","Cliente",$r->cliente));


$sel = new form_select("tipo_pedido","Tipo de Pedido");
$opts[] = array ("value" => 0 , "caption" => "Puertas");
$opts[] = array ("value" => 1 , "caption" => "Ventanas");
$opts[] = array ("value" => 2 , "caption" => "Puertas+Ventanas");
$sel->addOptions($opts);
unset($opts);
$f->addWidget($sel);
$f->addWidget(new form_separator("Fechas"));
$d = new date();
$d->timestamp = time();
$d->parse();
$f->addWidget(new form_html("Fecha de Pedido ",$d->string));
$ds = new dateselector("entrega_prevista");
$ds->drop($d);
$f->addWidget(new form_widget("Fecha Entrega Prevista","",$ds));
$f->addWidget(new form_separator("Datos adicionales"));
$f->addWidget(new form_textarea("otros_materiales","Otros materiales",$r->otros_materiales));
$f->addWidget(new form_textarea("observaciones","Observaciones",$r->observaciones));
$b->addWidget($f);
$b->addWidget($t);
$p->addWidget($b);
$p->draw();
?>