$(function() {
	$(".btn").click(function() {
		$(".form-signin").toggleClass("form-signin-left");
    $(".form-signup").toggleClass("form-signup-left");
    $(".frame").toggleClass("frame-long");
    $(".signup-inactive").toggleClass("signup-active");
    $(".signin-active").toggleClass("signin-inactive");
    $(".forgot").toggleClass("forgot-left");   
    $(this).removeClass("idle").addClass("active");
	});
});

$(function() {
	$(".btn-signup").click(function() {
  $(".nav-class").toggleClass("nav-up");
  $(".form-signup-left").toggleClass("form-signup-down");
  $(".success").toggleClass("success-left"); 
  $(".frame").toggleClass("frame-short");
	});
});

$(function() {
	$(".btn-signin").click(function() {
  $(".btn-animate").toggleClass("btn-animate-grow");
  $(".welcome").toggleClass("welcome-left");
  $(".cover-photo").toggleClass("cover-photo-down");
  $(".frame").toggleClass("frame-short");
  $(".profile-photo").toggleClass("profile-photo-down");
  $(".btn-goback").toggleClass("btn-goback-up");
  $(".forgot").toggleClass("forgot-fade");
	});
});
//
var board = new Array();
$(function (board) {
    var canvas = document.getElementById('hexagonCanvas');
    if (canvas == null) return;
    var hexHeight,
        hexRadius,
        hexRectangleHeight,
        hexRectangleWidth,
        hexagonAngle = 0.523598776, //30 градусов в радианах
        sideLength = 18, //длина стороны, пискелов
        boardWidth = 30 //ширина "доски" по вертикали
    hexHeight = Math.sin(hexagonAngle) * sideLength;
    hexRadius = Math.cos(hexagonAngle) * sideLength;
    hexRectangleHeight = sideLength + 2 * hexHeight;
    hexRectangleWidth = 2 * hexRadius;
    if (canvas.getContext) {
        var ctx = canvas.getContext('2d');
        ctx.fillStyle = "#000000";
        ctx.strokeStyle = "#ff132c";
        ctx.lineWidth = 2;
        board = drawBoard(boardWidth); //первичная отрисовка

        canvas.addEventListener("mousemove", function (eventInfo) { //слушатель перемещения мыши
            var x = (eventInfo.offsetX || eventInfo.layer) * canvas.width / canvas.scrollWidth;
            var y = (eventInfo.offsetY || eventInfo.layerY) * canvas.height / canvas.scrollHeight;
            var hexY = Math.floor(y / (hexHeight + sideLength));
            var hexX = Math.floor((x - (hexY % 2) * hexRadius) / hexRectangleWidth);
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            draw(board); //перерисовка на mousemove
            //На доске ли координаты мыши
            var obj = { x: hexX, y: hexY, color: ctx };
            if (board.some(item => (item.x === obj.x) && (item.y === obj.y))) {
                ctx.fillStyle = "#F08080";
                ctx.lineWidth = 4;
                drawHexagon(ctx, obj.x, obj.y, false);
                ctx.lineWidth = 2;
            }
        });
        canvas.addEventListener("mousedown", function (eventInfo) { //слушатель нажатий мыши
            var x = (eventInfo.offsetX || eventInfo.layer) * canvas.width / canvas.scrollWidth;
            var y = (eventInfo.offsetY || eventInfo.layerY) * canvas.height / canvas.scrollHeight;
            var hexY = Math.floor(y / (hexHeight + sideLength));
            var hexX = Math.floor((x - (hexY % 2) * hexRadius) / hexRectangleWidth);
            //document.getElementById("arrLength").textContent = board.length;
            ctx.clearRect(0, 0, canvas.scrollWidth, canvas.scrollHeight);
            //На доске ли координаты мыши
            var obj = { x: hexX, y: hexY, color: ctx };
            if (board.some(item => (item.x === obj.x) && (item.y === obj.y))) {
                index = board.findIndex(item => (item.x === obj.x) && (item.y === obj.y));
                if (board[index].isFill)
                    board[index].isFill = false;
                else board[index].isFill = true;
                board[index].color = "#F080FF";
            }
            draw(board);
        });
    }
    function draw(board) {
        for (var k = 0; k < board.length; k++) {
            drawHexagon(ctx, board[k].x, board[k].y, false);
            if (board[k].isFill) {
                ctx.fillStyle = board[k].color;
                drawHexagon(ctx, board[k].x, board[k].y, true);
            }
        }
    }
    function drawBoard(width) {
        board = new Array();
        var side = 10;
        var start = side - 1;
        //отступ в зависимости от количества ячеек
        var indent = 0;
        for (var j = 0; j < side * 2 - 1; j++) {
            if (j < side) {
                start++;
                if (start % 2 == 0)
                    indent++;
            }
            else {
                start--;
                if (start % 2 == 1)
                    indent--;
            }

            for (var i = width / 2 - start + indent; i < width / 2 + indent; i++) {
                drawHexagon(ctx, i, j, false);
                var obj = { x: i, y: j, color: ctx.fillStyle, isFill: false };
                board.push(obj);
            }
        }
        return board;
    }
    function drawHexagon(canvasContext, x, y, fill = false) {
        x = x * hexRectangleWidth + ((y % 2) * hexRadius);//приведение координат
        y = y * (hexHeight + sideLength);
        canvasContext.beginPath();
        canvasContext.moveTo(x + hexRadius, y);
        canvasContext.lineTo(x + hexRectangleWidth, y + hexHeight);
        canvasContext.lineTo(x + hexRectangleWidth, y + hexHeight + sideLength);
        canvasContext.lineTo(x + hexRadius, y + hexRectangleHeight);
        canvasContext.lineTo(x, y + sideLength + hexHeight);
        canvasContext.lineTo(x, y + hexHeight);
        canvasContext.closePath();
        if (fill) canvasContext.fill();
        else canvasContext.stroke();
    }
});
