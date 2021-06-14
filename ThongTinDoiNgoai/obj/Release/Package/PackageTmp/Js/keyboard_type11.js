// OPEN KEYBOARD
function openKeyboard() {
    toggle(document.getElementsByClassName('simple-keyboard')[0]);
    var x = document.getElementsByClassName('simple-keyboard')[0].getAttribute("keyboard-type");
    console.log(document.getElementsByClassName('simple-keyboard')[0]);
    var Keyboard = window.SimpleKeyboard.default;

    if (x == 'text') {
        var keyboard = new Keyboard({
            onChange: input => onChange(input),
            onKeyPress: button => onKeyPress(button,keyboard)
        });
    } else if (x == 'number') {
        var keyboard = new Keyboard({
            onChange: input => onChange(input),
            onKeyPress: button => onKeyPress(button,keyboard),
            layout: {
                default: ["1 2 3", "4 5 6", "7 8 9", "{shift} 0 _", "{bksp}"],
                shift: ["! / #", "$ % ^", "& * (", "{shift} ) +", "{bksp}"]
            },
            theme: "hg-theme-default hg-layout-numeric numeric-theme"
        });
    }

    /**
     * EVENT:
     */

    document.querySelector(".input").addEventListener("input2", event => {
        keyboard.setInput(event.target.value);
    });
}

///---------------new

document.addEventListener("click", (evt) => {
    console.log('get event');
    const flyoutElements = document.getElementsByClassName("inside_keyboard");
    var i = 0;
    for (var flyoutElement of flyoutElements) {
        console.log("Vao for");
        i++;
        let targetElement = evt.target; // clicked element

        do {
            console.log("Vao do");
            if (targetElement == flyoutElement) {
                console.log("Vao if");
        
        // This is a click inside. Do nothing, just return.
                console.log(flyoutElement);
                console.log("Clicked inside!");
                return;
    }
        // Go up the DOM
            targetElement = targetElement.parentNode;
    } while (targetElement);
    }
        // This is a click outside.
        //document.getElementById("flyout-debug").textContent = "Clicked outside!";
    console.log("Clicked outside!");
    removeAllClassAndHideAll();
});


function removeClassName(elem, name) {
    var remClass = elem.className;
    var re = new RegExp('(^| )' + name + '( |$)');
    remClass = remClass.replace(re, '$1');
    remClass = remClass.replace(/ $/, '');
    elem.className = remClass;
}

function removeAllClassAndHideExcept(keyboardId) {
        //remove all class keyboard
        //$(".keyboard").each(function( index ) {
        //    console.log( index + ": " + $( this ).text() );
        //    $( this ).removeClass("simple-keyboard");
        //    //$( this ).addClass( "simple-keyboard" );
        //});

    var elements = document.querySelectorAll('.keyboard');
    for (var element of elements) {
        //removeClassName(element,'simple-keyboard');
        element.classList.remove("simple-keyboard");
        element.classList.remove("inside_keyboard");
        if (element.id != keyboardId)
            element.style.display = 'none'; //ẩn bàn phím của đối tượng trước ngoại trừ this
    }
}

function removeAllClassAndHideAll() {
    var elements = document.querySelectorAll('.keyboard');
    for (var element of elements) {
        element.classList.remove("simple-keyboard");
        element.classList.remove("inside_keyboard");
        element.style.display = 'none'; //hide keyboard all item
    }
    //input tag
    var inputElements = document.getElementsByTagName('input');
    for (var element of inputElements) {
        element.classList.remove("inside_keyboard");
    }
}

function banphimso(textId, keyboardId, imgIcon) {
    console.log('ban phim so');
    //remove all class keyboard - and hide exception this element
    removeAllClassAndHideExcept(keyboardId);

    //add class for current
    var currentKeyboard = document.getElementById(keyboardId);
    var currentText = document.getElementById(textId);
    var currentImgIcon;

    currentKeyboard.classList.add("simple-keyboard");

    //add class for EVENT INSIDE KEYBOARD - click
    currentKeyboard.classList.add("inside_keyboard");
    currentText.classList.add("inside_keyboard");
    if (imgIcon != null) {
        currentImgIcon = document.getElementById(imgIcon);
        currentImgIcon.classList.add("inside_keyboard");
    }

    console.log(document.querySelectorAll('.keyboard').length); //len

    toggle(currentKeyboard);
    //var x = document.getElementById(keyboardId).getAttribute("keyboard-type");

    console.log(document.getElementById(textId));

    var Keyboard = window.SimpleKeyboard.default;

    var keyboard = new Keyboard({
        onChange: input => onChangeNew(input, textId),
            onKeyPress: button => onKeyPress(button,keyboard,keyboardId),
        layout: {
            default: ["1 2 3", "4 5 6", "7 8 9", "{shift} 0 _", "{bksp}"],
        shift: ["! / #", "$ % ^", "& * (", "{shift} ) +", "{bksp}"]
    },
        theme: "hg-theme-default hg-layout-numeric numeric-theme"
    });

    currentKeyboard.classList.add("keyboard");
    currentKeyboard.classList.add("inside_keyboard");
    console.log(document.getElementById(keyboardId));
    /**
     * EVENT:
     */

    document.getElementById(textId).addEventListener("input", event => {
        keyboard.setInput(event.target.value);
    });
}

function banphimchu(textId, keyboardId, imgIcon) {
    console.log('ban phim chu');
    //remove all class keyboard - and hide exception this element
    removeAllClassAndHideExcept(keyboardId);

    //add class for current
    var currentKeyboard = document.getElementById(keyboardId);
    var currentText = document.getElementById(textId);
    var currentImgIcon;

    currentKeyboard.classList.add("simple-keyboard");

    //add class for EVENT INSIDE KEYBOARD - click
    currentKeyboard.classList.add("inside_keyboard");
    currentText.classList.add("inside_keyboard");
    if (imgIcon != null) {
        currentImgIcon = document.getElementById(imgIcon);
        currentImgIcon.classList.add("inside_keyboard");
    }

    console.log(document.querySelectorAll('.keyboard').length); //len

    //
    toggle(currentKeyboard);
    //var x = document.getElementById(keyboardId).getAttribute("keyboard-type");

    console.log(document.getElementById(textId));

    var Keyboard = window.SimpleKeyboard.default;

    var keyboard = new Keyboard({
        onChange: input => onChangeNew(input, textId),
        onKeyPress: button => onKeyPress(button,keyboard,keyboardId)
    });

    currentKeyboard.classList.add("keyboard");
    currentKeyboard.classList.add("inside_keyboard");
    console.log(document.getElementById(keyboardId));
    /**
     * EVENT:
     */

    document.getElementById(textId).addEventListener("input", event => {
        keyboard.setInput(event.target.value);
    });
}

function onChangeNew(input, textId) {
    document.getElementById(textId).value = input;
    console.log("Input changed", input);
}

/////////////----

//console.log(keyboard);

function onChange(input) {
    document.querySelector(".input").value = input;
    console.log("Input changed", input);
}

function onKeyPress(button,keyboard,keyboardId) {
    console.log("Button pressed", button);

    /**
     * If you want to handle the shift and caps lock buttons
     */
    if (button === "{shift}" || button === "{lock}") handleShift(keyboard, keyboardId);

    if (button === "{enter}") {
        console.log("Enter Clicked! by keyboard id:" + keyboardId);
    }
}

function handleShift(keyboard,keyboardId) {
    let currentLayout = keyboard.options.layoutName;
    let shiftToggle = currentLayout === "default" ? "shift" : "default";

    keyboard.setOptions({
        layoutName: shiftToggle
    });

    //re-add class
    var currentKeyboard = document.getElementById(keyboardId);
    currentKeyboard.classList.add("keyboard");
    currentKeyboard.classList.add("inside_keyboard");
}