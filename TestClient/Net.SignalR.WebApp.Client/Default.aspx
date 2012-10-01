    <html xmlns="http://www.w3.org/1999/xhtml">

<script src ="Scripts/json2.js" type="text/jscript"></script>
<script src="Scripts/jquery-1.6.4.min.js" type="text/javascript"></script>
<script src="Scripts/jquery.signalR-0.5.3.min.js" type="text/javascript"></script>
<script src="/signalr/hubs" type="text/javascript"></script>
   <script type="text/javascript">

       $(function () {
           //           jQuery.support.cors = true;
           //           $.connection.hub.url = 'http://localhost:8081/signalr/';
           //           var collectionH = $.connection.collectionhub;

           //           $.connection.hub.start({ transport: 'longPolling', xdomain: true })
           //                    .done(function () { alert("Now connected!"); })
           //                    .fail(function () { alert("Could not Connect!"); });
           //           collectionH.subscribe("Product")
           //                    .done(function () { alert("Now subscribed to product!"); })
           //                    .fail(function () { alert("Could not Connect to Product!"); });

           //           collectionH.publish = function (data) {
           //               $('#messages').append('<li>' + data + '</li>');
           //           };


           var connection = $.hubConnection('http://localhost:8081/');
           proxy = connection.createProxy('DDPStreamWriter')
           connection.start()
                    .done(function () {
                        proxy.invoke('subscribe', 'allproducts','product');
                        $('#messages').append('<li>invoked subscribe</li>');
                    })
                    .fail(function () { alert("Could not Connect!"); });


                    proxy.on('flush', function (msg) {
                        $('#messages').append('<li>' + msg.prodName + '</li>');
                    });

       });
    </script>


<body>

    <div>
    <ul id="messages"></ul>
    <input id="broadcast" type="button">
    </div>

</body>
</html>
