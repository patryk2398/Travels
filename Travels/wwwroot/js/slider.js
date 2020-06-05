let number = 1;
let timer1 = 0;
let timer2 = 0;

function setSlide(numberSlide)
{
    clearTimeout(timer1);
    clearTimeout(timer2);
    number = numberSlide-1;
    hide();
    setTimeout("changeSlide()", 400);
}

function hide()
{
    $("#slider").fadeOut(400);
}

function changeSlide()
{
    number++;
    if(number > 3) number = 1;
    let file = "<img src=\"slajdy/slajd" + number + ".jpg\"/>";
    
    document.getElementById("slider").innerHTML = file;
    $("#slider").fadeIn(400);
    timer1 = setTimeout("changeSlide()", 5000);
    timer2 = setTimeout("hide()", 4600);
}