$(document).ready(function() {
	  $("#btnload").bind("click", function() {
         var url = $('#url').val();
		 $('#wordcount').empty();
		$('#top10words').empty();
		  $('#homepageItems').empty();
        $('#indicators').empty();
   $.ajax( {
       url: 'http://localhost:29181/extractwebpage/LoadUrl?url='+url,
       dataType: 'json',
       success: function(data) {
        var response = '';
		var WordRows = '<table><tr><th>Word</th><th>Count</th></tr>';
         var   indicator = '';
        for (var i = 0; i < data.images.length; i++) {
            response += '<div class="item"><img class="img-fluid" src="' + data.images[i] + '"></div>';
            indicator += '<li data-target="#myCarousel" data-slide-to="'+i+'"></li>';
        }
		 for (var i = 0; i < data.top10words.length; i++) {
            WordRows += '<tr><td>'+data.top10words[i].word +'</td><td>'+data.top10words[i].count +'</td></tr>';
        }
		WordRows += '</table>';
        $('#homepageItems').append(response);
        $('#indicators').append(indicator);
        $('.item').first().addClass('active');
        $('.carousel-indicators > li').first().addClass('active');
        $("#myCarousel").carousel();
		$('#wordcount').text(data.wordscount);
		$('#top10words').append(WordRows);
       }
   });
            });
	

});