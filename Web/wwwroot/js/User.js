$(() => {
    //SVG converter
    const svgConverter = function () {
        $("img.svg").each(function () {
            var $img = jQuery(this);
            var imgID = $img.attr("id");
            var imgClass = $img.attr("class");
            var imgURL = $img.attr("src");
            jQuery.get(
                imgURL,
                function (data) {
                    var $svg = jQuery(data).find("svg");
                    if (typeof imgID !== "undefined") {
                        $svg = $svg.attr("id", imgID);
                    }
                    if (typeof imgClass !== "undefined") {
                        $svg = $svg
                            .attr("class", imgClass + " replaced-svg")
                            .css("display", "inline-block");
                    }
                    $svg = $svg.removeAttr("xmlns:a");
                    $img.replaceWith($svg);
                },
                "xml"
            );
        });
    };
    svgConverter();

    $("img").on("error", function () {
        var resize = $(this).attr("src").split("?")[1];
        $(this).attr(
            "src",
            "/images/placeholder.jpg" +
            (resize != null
                ? "?" + resize
                : "?w=500&h=500&mode=crop&scale=both")
        );
        $(this).addClass("img-fluid");
    });

    // start Banner Slider
    const bannerSlider = $(".banner__slider");
    if (bannerSlider.length > 0) {
        bannerSlider
            .slick({
                slidesToShow: 1,
                slidesToScroll: 1,
                autoplay: true,
                speed: 2000,
                arrows: false,
                dots: true,
                responsive: [
                    {
                        breakpoint: 1,
                        settings: {
                            slidesToShow: 1,
                            slidesToScroll: 1,
                            dots: false,
                        },
                    },
                    {
                        breakpoint: 780,
                        settings: {
                            slidesToShow: 1,
                            slidesToScroll: 1,
                            dots: false,
                        },
                    },
                    {
                        breakpoint: 480,
                        settings: {
                            slidesToShow: 1,
                            slidesToScroll: 1,
                            dots: false,
                        },
                    },
                ],
            })
            .slickAnimation();
    }

    // start clients slider
    const clientsSlider = $(".clients-slider");
    if (clientsSlider.length > 0) {
        clientsSlider.on("init", function (event, slick) {
            svgConverter();
        });
        clientsSlider.slick({
            slidesToShow: 6,
            slidesToScroll: 1,
            // centerMode: true,
            // focusOnSelect: true,
            arrows: false,
            dots: true,
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 1,
                        centerMode: false,
                        infinite: true,
                        dots: true,
                    },
                },
                {
                    breakpoint: 790,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1,
                        centerMode: false,
                        dots: true,
                    },
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        centerMode: false,
                        dots: false,
                    },
                },
            ],
        });
    }

    //

    // start testimonials slider
    const testimonialsSlider = $(".testimonials-slider");
    if (testimonialsSlider.length > 0) {
        testimonialsSlider.slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            dots: true,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: true,
                    },
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: true,
                    },
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: false,
                    },
                },
            ],
        });
    }
    // start vendors slider
    const vendorsSlider = $(".vendors-slider");
    if (vendorsSlider.length > 0) {
        vendorsSlider.slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            dots: true,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: false,
                    },
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: false,
                    },
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        infinite: true,
                        dots: false,
                    },
                },
            ],
        });
    }
    // start stickysidebar

    $(".sidebar").stickySidebar({
        topSpacing: 60,
        bottomSpacing: 60,
    });

    // start stickysidebar

    // end vendors slider
    // start slider-for and slider-nav
    $(".slider-for").slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: false,
        asNavFor: ".slider-nav",
    });
    $(".slider-nav").slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        asNavFor: ".slider-for",
        dots: true,
        focusOnSelect: true,
    });

    // end slider-for and slider-nav
});

// start mov nav left

$(".nav-icon").click(function () {
    $(".nav-move").addClass("nav-back");

    $(".overlay").css({
        opacity: "1",
        visibility: "visible",
    });
    $("body").css("overflow", "hidden");
    $(".nav-icon i").fadeOut(100);
});
$(".close i").click(function () {
    $(".nav-move").removeClass("nav-back");

    $(".overlay").css({
        opacity: "0",
        visibility: "hidden",
    });
    $("body").css("overflow", "visible");
    $(".nav-icon i").fadeIn(100);
});

//
// start add nav-bar active and remove frome siblings

$(".main-nav ul li ").click(function () {
    $(this).addClass("active").siblings().removeClass("active");
});

//
// start counterup

var incrementPlus;
var incrementMinus;

var buttonPlus = $(".cart-qty-plus");
var buttonMinus = $(".cart-qty-minus");

var incrementPlus = buttonPlus.click(function () {
    var $n = $(this)
        .parent(".button-container")
        .parent(".container")
        .find(".qty");
    $n.val(Number($n.val()) + 1);
});

var incrementMinus = buttonMinus.click(function () {
    var $n = $(this)
        .parent(".button-container")
        .parent(".container")
        .find(".qty");
    var amount = Number($n.val());
    if (amount > 0) {
        $n.val(amount - 1);
    }
});

//
// start dropdown

document
    .querySelectorAll(".navbar-nav .navbar-collapse .dropdown")
    .forEach(function (everydropdown) {
        everydropdown.addEventListener("hidden.bs.dropdown", function () {
            // after dropdown is hidden, then find all submenus
            this.querySelectorAll(".submenu").forEach(function (everysubmenu) {
                // hide every submenu as well
                everysubmenu.style.display = "none";
            });
        });
    });

document
    .querySelectorAll(".navbar-nav .navbar-collapse .dropdown-menu a")
    .forEach(function (element) {
        element.addEventListener("click", function (e) {
            let nextEl = this.nextElementSibling;
            if (nextEl && nextEl.classList.contains("submenu")) {
                // prevent opening link if link needs to open dropdown
                e.preventDefault();
                if (nextEl.style.display == "block") {
                    nextEl.style.display = "none";
                } else {
                    nextEl.style.display = "block";
                }
            }
        });
    });

// end dropdown

function DuplicateRow($url) {
    return new swal({
        title: "Your Item will be Duplicate!",
        text: "Are you sure to proceed?",
        type: "success",
        buttons: true,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Duplicate My Item!",
        cancelButtonText: "I am not sure!",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((isConfirm) => {
        if (isConfirm) {
            $.ajax({
                type: "POST",
                url: $url,
                success: function (data) {
                    if (data == "Duplicated") {
                        new swal("Item Duplicated!", "Your Item is Duplicated!", "success");
                        setTimeout(function () {
                            location.reload();
                        }, 500);
                    } else {

                        new swal("Hurray", "Item is not Duplicated!", "error");
                    }

                }
            });

        }
        else {
            new swal("Hurray", "Item is not Duplicated!", "error");
        }
    });

}
function DeleteRow($url) {
    return new swal({
        title: "Your Item will be deleted!",
        text: "Are you sure to proceed?",
        type: "warning",
        buttons: true,
        showCancelButton: true,
        cancelButtonColor: "#DD6B55",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Remove My Item!",
        cancelButtonText: "I am not sure!",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((isConfirm) => {
        if (isConfirm) {
            $.ajax({
                type: "POST",
                url: $url,
                success: function (data) {
                    if (data == "Removed") {
                        setTimeout(function () {
                            location.reload();
                        }, 500);
                    } else {
                        new swal("Item Removed!", "Your Item is removed!", "success");

                    }

                }
            });
            new swal("Item Removed!", "Your Item is removed!", "success");

        }
        else {
            new swal("Hurray", "Item is not removed!", "error");
        }
    });

}

$(function () {
    if ($('input.tag').length > 0) {
        $('input.tag').each(
            function (i, obj) {
                $(obj).selectize({
                    plugins: ['remove_button'],
                    persist: false,
                    createOnBlur: true,
                    create: true
                });
            });
    }
});



$(document).ready(function () {



    document.getElementById('contactusbutton').addEventListener('click', function () {
        if ($("#contactusform").valid()) {
            $(this).attr('disabled', true)
            $(this).parents('form').submit()
        }



    })


    //document.getElementById('contactusbuttonn').addEventListener('click', function () {
    //    if ($("#contactusformm").valid()) {
    //        $(this).attr('disabled', true)
    //        $(this).parents('form').submit()
    //    }

    //})


})



function DuplicateRow($url) {
    swal({
        title: "Your Item will be Duplicate!",
        text: "Are you sure to proceed?",
        type: "success",
        buttons: true,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Duplicate My Item!",
        cancelButtonText: "I am not sure!",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((isConfirm) => {
        if (isConfirm) {
            $.ajax({
                type: "POST",
                url: $url,
                success: function (data) {
                    if (data == "Duplicated") {
                        swal("Item Duplicated!", "Your Item is Duplicated!", "success");
                        setTimeout(function () {
                            location.reload();
                        }, 500);
                    } else {
                        swal("Hurray", "Item is not Duplicated!", "error");
                    }

                }
            });

        }
        else {
            swal("Hurray", "Item is not Duplicated!", "error");
        }
    });

}
function DeleteRow($url) {
    swal({
        title: "Your Item will be deleted!",
        text: "Are you sure to proceed?",
        type: "warning",
        buttons: true,
        showCancelButton: true,
        cancelButtonColor: "#DD6B55",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Remove My Item!",
        cancelButtonText: "I am not sure!",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((isConfirm) => {
        if (isConfirm) {
            $.ajax({
                type: "POST",
                url: $url,
                success: function (data) {
                    if (data == "Removed") {
                        setTimeout(function () {
                            location.reload();
                        }, 500);
                    } else {
                        swal("Item Removed!", "Your Item is removed!", "success");

                    }

                }
            });
            swal("Item Removed!", "Your Item is removed!", "success");

        }
        else {
            swal("Hurray", "Item is not removed!", "error");
        }
    });

}

$(function () {
    if ($('input.tag').length > 0) {
        $('input.tag').each(
            function (i, obj) {
                $(obj).selectize({
                    plugins: ['remove_button'],
                    persist: false,
                    createOnBlur: true,
                    create: true
                });
            });
    }
});








$(document).ready(function () {



    document.getElementById('contactusbutton').addEventListener('click', function () {
        if ($("#contactusform").valid()) {
            $(this).attr('disabled', true)
            $(this).parents('form').submit()
        }



    })


    //document.getElementById('contactusbuttonn').addEventListener('click', function () {
    //    if ($("#contactusformm").valid()) {
    //        $(this).attr('disabled', true)
    //        $(this).parents('form').submit()
    //    }

    //})


})





var exampleModal = document.getElementById('exampleModal')
exampleModal.addEventListener('show.bs.modal', function (event) {
    // Button that triggered the modal
    var button = event.relatedTarget
    // Extract info from data-bs-* attributes
    var recipient = button.getAttribute('data-bs-whatever')
    // If necessary, you could initiate an AJAX request here
    // and then do the updating in a callback.
    //
    // Update the modal's content.
    var modalTitle = exampleModal.querySelector('.modal-title')
    var modalBodyInput = exampleModal.querySelector('.modal-body input')

    modalTitle.textContent = 'New message to ' + recipient
    modalBodyInput.value = recipient
})




















