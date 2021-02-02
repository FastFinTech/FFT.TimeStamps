<script type="application/javascript">

function resizeIFrameToFitContent( iFrame ) {

    iFrame.width  = iFrame.contentWindow.document.body.scrollWidth;
    iFrame.height = iFrame.contentWindow.document.body.scrollHeight;
}

window.addEventListener('DOMContentLoaded', function(e) {

    var iFrame = document.getElementById( 'frame' );
    resizeIFrameToFitContent( iFrame );

    // or, to resize all iframes:
    //var iframes = document.querySelectorAll("iframe");
    //for( var i = 0; i < iframes.length; i++) {
    //    resizeIFrameToFitContent( iframes[i] );
    //}
} );

</script>

<iframe src="https://fastfintech.github.io/FFT.TimeStamps/index.html" id="frame"></iframe>

