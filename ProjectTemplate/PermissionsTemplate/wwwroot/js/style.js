$(document).ready(function(){ 
	$("#tableOne tr").each(function(index){
		$(this).click(function(){
            if($(this).find("td").hasClass('bgColor')){
                $(this).find("td").eq(1).removeClass("bgColor");				 			
            }else{
                $(this).find("td").eq(1).addClass("bgColor");
            }
		})
	});
	$("#tableTwo tr").each(function(index){
		$(this).click(function(){
            if($(this).find("td").hasClass('bgColor')){
                $(this).find("td").eq(1).removeClass("bgColor");
                $(this).find("input").prop("checked", false);
            }else{
                $(this).find("td").eq(1).addClass("bgColor");
                $(this).find("input").prop("checked", true);
            }
		})
	});
})