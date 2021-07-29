window.MathJax = {
    startup: {
        pageReady: () => {
            console.log("MathJax is loaded and initialized.");
        }
    }
};

window.minicas =
{
    setautoexpand: function (textarea) {
        textarea.addEventListener('input', autosize);

        function autosize() {
            textarea.style.height = 'auto';
            textarea.style.height = (input.scrollHeight) + 'px';
        }
    },
    setSelectionRange: function (textarea, start, end) {
        textarea.setSelectionRange(start, end);
    }
    ,
    processLatex: async function (input, isDisplay) {
        return MathJax.tex2chtmlPromise(input, { display: isDisplay }).then(function (node) {
            //
            //  The promise returns the typeset node, which we add to the output
            //  Then update the document to include the adjusted CSS for the
            //    content of the new equation.
            //
            MathJax.startup.document.clear();
            MathJax.startup.document.updateDocument();
            return node.outerHTML;

        }).catch(function (err) {
            //
            //  If there was an error, put the message into the output instead
            //
            return err.message;
        });
    }
};