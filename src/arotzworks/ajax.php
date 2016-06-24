<?php         
include ("local.php");   
header("content-type: text/xml");
echo "<?xml version=\"1.0\"?>";
echo "\n<results>";
$r = new request();
$sf = new scaffold($r->entity);
$sf->setCondition($r->condition_entity,$r->condition_value);
foreach ($sf->getList() as $item)
{                                                           
		echo "\n\t<result id=\"".$item["id"]."\" name=\"".$item["referencia"]."\" />";
}
echo "\n</results>";