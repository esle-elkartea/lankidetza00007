<?php
include ("local.php");

$p = new aworkspage();
error_reporting(E_ALL);
$sf = new scaffold("pedidos");
$sf->open($r->pedido_id);
if ($r->referencia)
{
	$sf->fromObject($r);
	$sf->update();
}
$b = new Box("Datos Generales del Pedido");
$t = new toolbar();
$t->addWidget(new button("Guardar Datos","javascript:document.forms[0].submit()","disk"));
$t->addWidget(new button("Actualizar Estado del Pedido","pedido_evento_add.php?pedido_id=".$r->pedido_id,"lightning"));
$b->addWidget($t);
$f = new form("","");
$f->addWidget(new form_separator("Referencia y Cliente"));
$f->addWidget(new form_input("referencia","Referencia",$sf->referencia));
$f->addWidget(new form_html("Tracking","<span style=\"font-size:14px;font-weight:bold;\">".$sf->tracking));
$f->addWidget(new form_input("cliente","Cliente",$sf->cliente));
$f->addWidget(new form_separator("Fechas"));
$d = new date();
$d->timestamp = $sf->fecha_pedido;
$d->parse();
$f->addWidget(new form_html("Fecha de Pedido ",$d->string));
$d = new date();
$d->timestamp = $sf->fecha_entrega_prevista;
$d->parse();
$f->addWidget(new form_html("Fecha Entrega Prevista",$d->string));
$d = new date();
$d->timestamp = $sf->fecha_entrega;
$d->parse();
$f->addWidget(new form_html("Fecha Entrega",$d->string));
$elw = new estadolistwidget();
$elw->drop($sf);
$f->addWidget(new form_widget("Estado","",$elw));
$f->addWidget(new form_separator("Datos adicionales"));
$f->addWidget(new form_textarea("otro_material","Otros materiales",$sf->otro_material));
$f->addWidget(new form_textarea("observaciones","Observaciones",$sf->observaciones));

$b2 = new Box("Datos Generales del Pedido");
$b2->addWidget($f);
$b3 = new Box("Eventos del Pedido");
$ev = new eventviewer();
$ev->drop($sf);
$b3->addWidget($ev);
$b->addWidget(html::write("<table style=\"width:100%;\"><tr><td valign=\"top\" style=\"width:40%;\">".$b2->draw()."</td><td  valign=\"top\">".$b3->draw()."</td></tr></table>"));
$b->addWidget(html::write("<script> var idpedido='".$sf->id."';</script>"));
$p->addWidget($b);
$b = new Box("Detalle del Pedido");
if ($sf->tipo_pedido == 0 || $sf->tipo_pedido == 2) $b->addWidget(new puertas_paso($sf->estado < 1 ? true : false));
if ($sf->tipo_pedido == 0 || $sf->tipo_pedido == 2) $b->addWidget(new puertas_entrada($sf->estado < 1 ? true : false));
if ($sf->tipo_pedido == 1 || $sf->tipo_pedido == 2) $b->addWidget(new ventanas($sf->estado < 1 ? true : false));
$p->addWidget($b);
$p->draw();
