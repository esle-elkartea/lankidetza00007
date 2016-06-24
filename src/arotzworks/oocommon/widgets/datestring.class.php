<?php

	class datestring extends oowidget
	{
		var $timestamp;
		var $string;
			
		function datestring($timestamp)
		{
			$d = new date();	
			$d->timestamp = $timestamp;
			$d->parse();
			$this->timestamp = $timestamp;
			$this->string = $d->string;
		}
		function draw()
		{
			return $this->string;
		}

	}