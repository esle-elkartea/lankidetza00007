<?php
include ("local.php");
$sf = new scaffold("pedidos_ventanas");
$sf->fromObject($r);
$sf->insert();
print_arr(errorobject::errorlist());
echo "ok";
?>