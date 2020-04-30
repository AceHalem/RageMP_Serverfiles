	$('#callTaxi').click(() => {
		$('.alert').remove();
		mp.trigger('callTaxiNow');
	});
