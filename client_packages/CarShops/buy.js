	$('#BuySurfer').click(() => {
		$('.alert').remove();
		mp.trigger('BuySurferVeh');
	});
	$('#BuyInjection').click(() => {
		$('.alert').remove();
		mp.trigger('BuyInjectionVeh');
	});
	$('#BuyEmperor').click(() => {
		$('.alert').remove();
		mp.trigger('BuyEmperorVeh');
	});
	$('#BuyBodhi').click(() => {
		$('.alert').remove();
		mp.trigger('BuyBodhiVeh');
	});
	
	//Fahrzeughändler
	$('#BuyNovak').click(() => {
		$('.alert').remove();
		mp.trigger('BuyNovakVeh');
	});
	$('#BuyCaracara').click(() => {
		$('.alert').remove();
		mp.trigger('BuyCaracaraVeh');
	});
	$('#BuyDrafter').click(() => {
		$('.alert').remove();
		mp.trigger('BuyDrafterVeh');
	});
	$('#BuySchlagen').click(() => {
		$('.alert').remove();
		mp.trigger('BuySchlagenVeh');
	});