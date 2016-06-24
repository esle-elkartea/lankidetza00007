<?php
include ("local.php");
$p = new aworkspage();
$p->addWidget(html::write("<h1>Gesti&oacute;n de Modelos</h1>"));
$app = new dataapp("modelos","modelos.php","");
$p->addWidget($app);
$p->draw();
