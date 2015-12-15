jQuery.cookie = function(e, f, a) {
    if ("undefined" != typeof f) {
        a = a || {};
        null === f && (f = "", a.expires =- 1);
        var g = "";
        if (a.expires && ("number" == typeof a.expires || a.expires.toUTCString))
            "number" == typeof a.expires ? (g = new Date, g.setTime(g.getTime() + 864E5 * a.expires)) : g = a.expires, g = "; expires=" + g.toUTCString();
        var i = a.path ? "; path=" + a.path: "", k = a.domain ? "; domain=" + a.domain: "", a = a.secure ? "; secure": "";
        document.cookie = [e, "=", encodeURIComponent(f), g, i, k, a].join("")
    } else {
        f = null;
        if (document.cookie && "" != document.cookie) {
            a =
            document.cookie.split(";");
            for (g = 0; g < a.length; g++)
                if (i = jQuery.trim(a[g]), i.substring(0, e.length + 1) == e + "=") {
                    f = decodeURIComponent(i.substring(e.length + 1));
                    break
                }
        }
        return f
    }
};
window.SA || (SA = {});
SA.utils = new function(e, f, a) {
    var g = this;
    g.storageEnabled = e.localStorage?!0 : !1;
    g.isMobile = /android|iphone|ipod|ipad|webos|blackberry/i.test(navigator.userAgent);
    g.store = function(a) {
        if (g.storageEnabled && null !== a) {
            var e = null;
            if (1 < arguments.length)
                e = arguments[1], null === e ? localStorage.removeItem(a) : localStorage.setItem(a, JSON.stringify(e));
            else if (e = localStorage.getItem(a), null !== e)
                return JSON.parse(e)
        }
        return null
    };
    g.qs = function(a) {
        if (a) {
            var g = [], c;
            for (c in a)
                g.push(encodeURIComponent(c) + "=" + encodeURIComponent(a[c]));
            return g.join("&")
        }
        a = {};
        if (g = e.location.search)
            for (var g = g.substr(1).split("&"), j = 0; j < g.length; j++)
                c = g[j].indexOf("="), - 1 == c ? a[g[j]] = "" : a[decodeURIComponent(g[j].substr(0, c))] = decodeURIComponent(g[j].substr(c + 1));
        return a
    };
    e.rf = function(e) {
        a("td.postbody iframe").css("height", Math.min(e, 800))
    }
}(window, document, jQuery);
new function(e, f, a) {
    var g = "html", i, k = 0, c = 0, j = function(a) {
        var c = Math.floor(160 * Math.random()) + 1;
        return "http://fi.somethingawful.com/sideimages/" + a + "88/" + c + ".jpg"
    }, b = function() {
        i.offset();
        var b = a(g).scrollTop(), d = {
            position: "absolute",
            top: k
        }, e = a("div#content"), j = e.offset().top, e = e.height(), j = j + e - c;
        b > j ? e > c && (d.top = j) : b >= k && (a.browser.msie && 7 > parseInt(a.browser.version, 10) ? d.top = b : (d.position = "fixed", d.top = 0));
        i.css(d)
    };
    a(f).ready(function() {
        var h = a("div.oma_pal"),
        d = a(new Image);
        d.attr({
            width: 88,
            height: 88,
            "class": "omapalpet"
        });
        d.css("display", "block");
        h.each(function(c, b) {
            var b = a(b), h = d.clone();
            h.addClass("left");
            h.attr("src", j("l"));
            b.prepend(h);
            h = d.clone();
            h.addClass("right");
            h.attr("src", j("r"));
            b.append(h)
        });
        i = a("div#unregskyscraper");
        i.length && (k = i.offset().top - 10, c = i.height() + 10, a(e).scroll(b));
        a("iframe.adframe").each(function(c, b) {
            a(b).attr("src", "/adframe.php?z=" + a(b).attr("data-zone"))
        })
    })
}(window, document, jQuery);
function newposts(e) {
    window.location.href = "/showthread.php?goto=newpost&threadid=" + e
}
function validate_pm(e, f) {
    return "" == e.message.value || "" == e.touser.value ? (alert("Please complete the recipient and message fields."), !1) : 0 != f && e.message.value.length > f / 2 ? (alert("Your message is too long.\n\nReduce your message to " + f / 2 + " characters.\nIt is currently " + e.message.value.length + " characters long."), !1) : !0
}
function confirm_newpm() {
    input_box = confirm("You have a new private message. Click OK to view it, or cancel to hide this prompt.");
    !0 == input_box && (second_box = confirm("Open in new window?\n\n(Press cancel to open in the current window.)"), !0 == second_box ? window.open("private.php", "pmnew") : window.location = "private.php")
}
function posticon_sel(e) {
    document.vbform.iconid.item(e).checked=!0
}
function validate(e, f) {
    if (e.elements.namedItem("subject") && "" == e.subject.value)
        return alert("Please complete the subject field, shithead."), !1;
    var a = e.elements.namedItem("message");
    return a && "" == e.message.value ? (alert("Please complete the message field, shithead."), !1) : 0 != f && a.length > f ? (alert("Your message is too long.\n\nReduce your message to " + f + " characters.\nIt is currently " + e.message.value.length + " characters long.\n  Are you trying to spam?\n  If so, then STOP!"), !1) : !0
}
function checklength(e, f) {
    f || (f = 0);
    message = 0 != f ? "\nThe maximum permitted length is " + f + " characters." : "";
    alert("Your message is " + e.message.value.length + " characters long." + message)
}
function rate_thread(e) {
    document.rateform.vote.value = e;
    document.rateform.submit()
}
function reloadCaptcha() {
    document.images.captcha.src = "captcha.php?" + Math.random()
}
$(document).ready(function() {
    var e = $("#thread table.post");
    $(e).each(function(a, e) {
        try {
            var f = $(e).find("td.userinfo").get(0), k = f.className.match(/\buserid\-(\d+)\b/)[1];
            $(f).data("userid", k);
            var c = e.id.match(/\bpost(\d+)\b/)[1];
            $(e).data({
                userid: k,
                postid: c
            });
            var j = $(e).find("tr td.postlinks ul.profilelinks"), b = 3 <= j.find("li").length ? 2: 1;
            $(j).find("li:eq(" + b + ")", j).after('\n<li><a href="/banlist.php?userid=' + k + '">Rap Sheet</a></li>')
        } catch (h) {}
    });
    var f = RegExp(/^\(USER WAS (?:BANNED|PUT ON PROBATION) FOR THIS POST\)$/);
    $(e).each(function(a, e) {
        try {
            $(e).find("td.postbody > b:last").filter(function(a, c) {
                return !!$(c).text().match(f)
            }).wrapInner('<a href="/banlist.php?userid=' + $(e).data("userid") + '" />')
        } catch (i) {}
    });
    $("td.postbody .cancerous").closest("td").hover(function() {
        $(".cancerous", this).addClass("hover")
    }, function() {
        $(".cancerous", this).removeClass("hover")
    })
});
function add_whoposted_links() {
    $("#forum.threadlist tr.thread").each(function(e, f) {
        try {
            var a = f.id.match(/^thread(\d+)$/)[1];
            $("td.replies", f).wrapInner('<a href="/misc.php?action=whoposted&amp;threadid=' + a + '" target="_blank" title="List users that posted in this thread" />')
        } catch (g) {}
    })
}
new function(e, f, a) {
    var g = [], i = /thread(\d+)/i;
    a(new Image).attr("src", "http://fi.somethingawful.com/style/bookmarks/star-off.gif");
    a(new Image).attr("src", "http://fi.somethingawful.com/style/bookmarks/spin3.gif");
    for (var k = 0; 3 > k; k++)
        g.push(a(new Image).attr("src", "http://fi.somethingawful.com/style/bookmarks/star" + k + ".gif"));
    a(f).ready(function() {
        var c = a("tr.thread td.star");
        c.length && c.each(function(c, b) {
            var b = a(b), h = b.append("<div></div>"), d = b.parents("tr").eq(0), j = i.exec(d.attr("id"));
            if (j) {
                j =
                j[1];
                h.click(function() {
                    var c = a(this);
                    if (d.hasClass("spin"))
                        return !1;
                    d.addClass("spin");
                    d.removeClass("category0 category1 category2");
                    c.removeClass("bm0 bm1 bm2");
                    a.post("/bookmarkthreads.php", {
                        threadid: j,
                        action: "cat_toggle",
                        json: 1
                    }, function(a) {
                        0 <= a.category_id&&!e.disable_thread_coloring && d.addClass("category" + a.category_id);
                        d.removeClass("spin");
                        c.addClass("bm" + a.category_id)
                    });
                    return !1
                });
                var g = d.find("div.lastseen"), f = g.find("a.x");
                f.click(function() {
                    if (f.data("busy"))
                        return !1;
                    f.data("busy", !0);
                    f.html("<");
                    a.post("/showthread.php", {
                        threadid: j,
                        action: "resetseen",
                        json: 1
                    }, function(a) {
                        a.threadid && (d.removeClass("seen"), g.remove())
                    });
                    return !1
                })
            }
        });
        var j=!1, b = a("div#bookmark_link a"), h = a("img.thread_bookmark");
        if (b.length && h.length) {
            var d = parseInt(a("body").attr("data-thread"), 10), g = function() {
                h.hasClass("unbookmark") ? b.html("Unbookmark this thread") : b.html("Bookmark this thread")
            }, f = function() {
                h.hasClass("bookmark") ? h.attr({
                    src: "http://fi.somethingawful.com/images/buttons/button-bookmark.png",
                    alt: "Bookmark",
                    title: "Bookmark thread"
                }) : h.attr({
                    src: "http://fi.somethingawful.com/images/buttons/button-unbookmark.png",
                    alt: "Unbookmark",
                    title: "Unbookmark thread"
                })
            }, c = function() {
                if (j)
                    return !1;
                j=!0;
                var c = {
                    action: h.hasClass("unbookmark") ? "remove": "add",
                    threadid: d,
                    json: 1
                };
                a.post("/bookmarkthreads.php", c, function(a) {
                    h.removeClass("bookmark unbookmark");
                    a.bookmarked ? h.addClass("unbookmark") : h.addClass("bookmark");
                    f();
                    g();
                    j=!1
                })
            };
            h.click(c);
            b.click(c);
            f();
            g()
        }
        if (a("body").hasClass("usercp") || a("body").hasClass("bookmark_threads"))
            c =
            a("table#forum"), c.length && (c.find("thead > tr").append("<th></th>"), c.find("tbody > tr").append('<td class="button_remove"><div title="Remove bookmark"></div></td>'), c.delegate("td.button_remove div", "click", function() {
                var c = a(this), b = c.parents("tr").eq(0), d = i.exec(b.attr("id"));
                if (d) {
                    d = d[1];
                    if (c.data("pending"))
                        return !1;
                        c.data("pending", !0);
                        c.removeClass("warn");
                        c.addClass("spin");
                        a.post("/bookmarkthreads.php", {
                            threadid: d,
                            action: "remove",
                            json: 1
                        }, function() {
                            b.remove()
                        })
                }
                return !1
            }))
    })
}(window, document,
jQuery);
new function(e, f, a) {
    a(f).ready(function() {
        var e = a("div.threadrate"), f = "THANK GOD YOU VOTED!;Just the vote we were looking for.;You're a real gem.;Thanks toots.;*beep boop* vote accepted *bzzt*;We threw your vote into the pile.;Thank you, citizen!;That was the best vote I have ever seen.;That was a really good vote.;Vote accepted!  Thanks a lot!;Vot acepteed^ thinks aot;Thanks champ!".split(";");
        if (e.length) {
            var k = parseInt(a("body").attr("data-thread"), 10);
            0 >= k || isNaN(k) || e.delegate("ul.rating_buttons li", "click",
            function() {
                var c = a(this).index() + 1;
                a.post("/threadrate.php", {
                    threadid: k,
                    vote: c
                }, function() {
                    e.html(f[Math.floor(Math.random() * f.length)])
                })
            })
        }
    })
}(window, document, jQuery);
new function(e, f, a) {
    var g = "_sessl", g = g + "", i = function(a) {
        return parseInt(a, 10) + 0
    }, k = function(a) {
        for (var a = a.substr(1).split("&"), e = 0, b, h = {}, d = a.length, f; e < d; e++)
            b = a[e].indexOf("="), - 1 != b && (f = a[e].substr(0, b), b = a[e].substr(b + 1), h[f] = b);
        return h
    };
    a(f).ready(function() {
        if (a("div#notregistered").length) {
            a("table#forum th a").each(function(c, b) {
                a(b).replaceWith(b.childNodes)
            });
            a("ul.postbuttons img#button_bookmark").parent().remove();
            var c = e.location.search;
            if (c) {
                var f = k(c), b = c = 0, h = 0;
                if (f.hasOwnProperty("forumid"))
                    c =
                    i(f.forumid);
                else {
                    var d = a('div.breadcrumbs a[href^="forumdisplay.php"]').last();
                    d.length && (d = d.attr("href"), d = k(d.substr(d.indexOf("?"))), d.hasOwnProperty("forumid") && (c = i(d.forumid)))
                }
                f.hasOwnProperty("threadid") && (b = i(f.threadid));
                f.hasOwnProperty("postid") && (h = i(f.postid));
                var f=!0, d = i((new Date).getTime() / 1E3), n = a.cookie(g);
                if (n && (value = n.split("."), 4 == value.length)) {
                    var n = i(value[0]), t = i(value[1]), w = i(value[2]), l = i(value[3]);
                    180 > d - l && (f=!1, n == c && t == b && w == h || (f=!0))
                }
                f && (c = [c, b, h, d].join("."),
                a.cookie(g, c, {
                    expires: 10,
                    domain: "forums.somethingawful.com"
                }), a("body").append('<img src="/s/' + c + '" alt="">'))
            }
        }
    })
}(window, document, jQuery);
window.SA || (SA = {});
SA.timg = new function(e, f, a) {
    var g = this, i = function(c, f) {
        var b = a(this).siblings("img"), h, d;
        if (b.attr("t_width"))
            a(this).removeClass("expanded"), b.attr({
                width: b.attr("t_width"),
                height: b.attr("t_height")
            }), b.removeAttr("t_width"), b.removeAttr("t_height");
        else {
            a(this).addClass("expanded");
            b.attr({
                t_width: b.attr("width"),
                t_height: b.attr("height")
            });
            var g = b.parents("blockquote");
            g.length || (g = b.parents(".postbody"));
            h = parseInt(b.attr("o_width"), 10);
            d = parseInt(b.attr("o_height"), 10);
            g = Math.min(900, g.width());
            if (f && h > g) {
                var i = b.position(), g = (g - 3 * i.left) / h;
                b.attr("width", h * g);
                b.attr("height", d * g)
            } else 
                b.removeAttr("width"), b.removeAttr("height");
            g = a.browser.webkit || a.browser.safari ? "body" : "html";
            d = a(g).scrollTop();
            h = b.offset().top;
            b = h + b.height();
            b - d > a(e).height() && (d = b - a(e).height());
            h < d && (d = h);
            d != a(g).scrollTop() && (a.browser.msie && 7 > parseInt(a.browser.version, 10) ? a(g).scrollTop(d) : a(g).animate({
                scrollTop: d
            }, 150))
        }
        return !1
    }, k = function() {
        var c = a(this);
        if (c.hasClass("loading")) {
            c.removeClass("loading");
            var e =
            c[0].naturalWidth || c.width(), b = c[0].naturalHeight || c.height();
            if (200 > b && 500 >= e || 170 > e)
                c.removeClass("timg");
            else {
                c.addClass("complete");
                c.attr({
                    o_width: e,
                    o_height: b
                });
                var e = e + "x" + b, b = 1, h = c[0].naturalWidth || c.width(), d = c[0].naturalHeight || c.height();
                170 < h && (b = 170 / h);
                200 < d * b && (b = 200 / d);
                c.attr({
                    width: h * b,
                    height: d * b
                });
                var b = a('<span class="timg_container"></span>'), f = a('<div class="note"></div>');
                f.text(e);
                f.attr("title", "Click to toggle");
                b.append(f);
                c.before(b);
                b.prepend(c);
                f.click(i);
                b.click(function(c) {
                    if (1 ===
                    c.which || a.browser.msie && 9 > parseInt(a.browser.version, 10) && 0 === c.which)
                        return i.call(f, c, !0), !1
                })
            }
            c.trigger("timg.loaded")
        }
    };
    g.scan = function(c) {
        a(c).find("img.timg").each(function(c, b) {
            b = a(b);
            b.hasClass("complete") || (b.addClass("loading"), b[0].complete || null !== b[0].naturalWidth && 0 < b[0].naturalWidth ? k.call(b) : b.load(k))
        })
    };
    a(f).ready(function() {
        g.scan("body")
    });
    a(e).load(function() {
        var c = a("img.timg.loading");
        c.length && c.each(function(a, c) {
            k.call(c)
        })
    })
}(window, document, jQuery);
(function(e, f, a) {
    var g=!1, i = [], k = {}, c = null, j = null, b = [], h = 0, d = 0, n = 0, t = 0, w = "", l = null, m = null, x=!1, A = e.localStorage?!0 : !1, N = e.JSON && JSON.stringify && JSON.parse, r=!1, y=!1, G=!1, B=!1, C = 0, D = function(a) {
        if (A && null !== a) {
            var c = null, a = "postreply_" + a;
            if (1 < arguments.length)
                c = arguments[1], null === c ? localStorage.removeItem(a) : localStorage.setItem(a, JSON.stringify(c));
            else if (c = localStorage.getItem(a), null !== c)
                return JSON.parse(c)
        }
        return null
    }, H = function(a) {
        var b = "\n";
        "\n" != c[0].value.substr(c[0].value.length - 1) && (b +=
        "\n");
        s(b + a, null, !1)
    }, E = function() {
        if (!g && i.length) {
            g=!0;
            var c = a(i.shift());
            k[c.attr("href")] ? (H(k[c.attr("href")]), E()) : a.get(c.attr("href") + "&json=1", {}, function(a) {
                k[c.attr("href")] = a.body;
                H(a.body);
                c.children("img").attr("src", "http://fi.somethingawful.com/images/sa-quote-added.gif");
                E()
            }, "json")
        } else 
            g=!1
    }, O = function() {
        a(this).css("opacity", 0.6);
        i.push(this);
        E();
        return !1
    }, I = function() {
        if (A && (null === b || 1 < b.length)) {
            var e = new Date, d = null;
            if (r)
                d = "forum-" + t;
            else if (y)
                d = "reply-" + n;
            else 
                return;
            l.find("div.save-state").text("Saved: " +
            e.toLocaleDateString() + " " + e.toLocaleTimeString());
            D(d, {
                time: e.getTime(),
                message: a.trim(c[0].value)
            })
        }
    }, z = function() {
        clearInterval(d);
        if (null !== b) {
            h != b.length - 1 && b.splice(h + 1, b.length - h - 1);
            var a = q();
            b.push({
                selection: a,
                scroll: c[0].scrollTop,
                text: c[0].value
            });
            h = b.length - 1
        }
        I()
    }, J = function() {
        if (b.length && 0 <= h && h < b.length) {
            var a = b[h].selection;
            c[0].value = b[h].text;
            u(a.start, a.end);
            c[0].scrollTop = b[h].scroll;
            p();
            I()
        }
    }, K = function() {
        h = Math.min(b.length - 1, h + 1);
        J()
    }, v = function() {
        clearInterval(d);
        d = setTimeout(z,
        250)
    }, q = function() {
        var a = null;
        "new" == x && (a = {
            start: c[0].selectionStart,
            end: c[0].selectionEnd
        });
        return a
    }, u = function(a) {
        c[0].setSelectionRange(a, 2 == arguments.length ? arguments[1] : a)
    }, L = function(a) {
        for (var c = [[0, 47], [58, 64], [91, 96], [123, 126]], b = 0, e = c.length, d=!1; b < e; b++)
            if (a >= c[b][0] && a <= c[b][1]) {
                d=!0;
                break
            }
        return d
    }, M = function(a) {
        var b = q(), e = c[0].value, d = e.lastIndexOf("[" + a, b.start);
        return - 1 != d ? (d = e.indexOf("[/" + a + "]", d), - 1 == d?!1 : d + 3 > b.end) : !1
    }, s = function(a, b, e) {
        null === b && (b = q());
        var d = c[0].scrollTop,
        h = c[0].value, f = h.substring(0, b.start), g = h.substring(b.start, b.end), h = h.substr(b.end), i = b.start;
        e ? (e = "[" + a + "]" + g + "[/" + a + "]", c[0].value = f + e, i = b.start == b.end ? b.start + a.length + 2 : b.start + e.length) : (c[0].value = f + a, i = b.start + a.length);
        b.end = i;
        c[0].scrollTop = c[0].scrollHeight;
        c[0].value += h;
        c[0].scrollTop < d && (c[0].scrollTop = d);
        u(i);
        z();
        p();
        return b
    }, F = function(a) {
        var a = a.split("&"), c = {}, b, e;
        for (e in a)
            b = a[e].indexOf("="), - 1 != b ? c[a[e].substr(0, b)] = a[e].substr(b + 1) : c[a[e]]=!0;
        return c
    }, P = function() {
        var a = q(),
        b = null;
        return 5 <= a.start && (b = c[0].value.substr(a.start - 5), /^(\[img\]|\[url\]|\[url="?)/.test(b)) || 6 <= a.start && (b = c[0].value.substr(a.start - 6), /^\[timg\]/.test(b))?!0 : !1
    }, Q = function(a) {
        var b = null, e = "";
        if (a.originalEvent.metaKey || a.originalEvent.ctrlKey) {
            var d = null;
            switch (a.keyCode) {
            case 66:
                d = "b";
                break;
            case 73:
                d = "i";
                break;
            case 76:
                var d = q(), f = c[0].value, g = f.substring(0, d.start), e = f.substring(d.start, d.end), i = "[list]\n[*]" + e + "\n[/list]", f = f.substr(d.end);
                c[0].value = g + i + f;
                u(d.start + e.length + 10);
                z();
                p();
                a.stopPropagation();
                a.preventDefault();
                return !1;
            case 81:
                d = "quote";
                break;
            case 83:
                d = "s";
                break;
            case 85:
                d = "u";
                break;
            case 86:
                if (P())
                    break;
                b = q();
                j.val("");
                j.focus();
                setTimeout(function() {
                    var a = b, d = j[0].value;
                    if (/^https?:\/\//.test(d)&&-1 == d.indexOf("\n")&&-1 == d.indexOf("\r")) {
                        var e;
                        var h = /([^:]+):\/\/([^\/]+)(\/.*)?/.exec(decodeURI(d));
                        if (h) {
                            var f = {
                                scheme: h[1],
                                domain: h[2],
                                path: h[3] || "",
                                filename: "",
                                query: {},
                                fragment: ""
                            }, h = f.path.lastIndexOf("#");
                            - 1 != h && (f.fragment = f.path.substr(h + 1), f.path = f.path.substr(0,
                            h));
                            h = f.path.lastIndexOf("?");
                            - 1 != h && (f.query = F(f.path.substr(h + 1)), f.path = f.path.substr(0, h));
                            h = f.path.lastIndexOf("/");
                            - 1 != h && (f.filename = f.path.substr(h + 1));
                            e = f
                        } else 
                            e = null;
                        var f = h = "", g=!1, i = e.filename.lastIndexOf(".");
                        - 1 != i && (h = e.filename.substr(i + 1), f = e.filename.substr(0, i));
                        if (/^([^\.]+\.)?youtube(-nocookie)?\.com$/.test(e.domain) || /^([^\.]+\.)?youtu\.be$/.test(e.domain))
                            e.query.v ? (d = '[video type="youtube"', e.query.hd && (d += ' res="hd"'), e.fragment && (g = F(e.fragment), g.t && (d += ' start="' + parseInt(g.t,
                            10) + '"')), d += "]" + e.query.v + "[/video]") : /^\/embed/.test(e.path) ? (d = '[video type="youtube"', e.query.hd && (d += ' res="hd"'), e.query.start && (d += ' start="' + parseInt(e.query.start, 10) + '"'), d += "]" + e.path.substr(e.path.lastIndexOf("/") + 1) + "[/video]") : (d = '[video type="youtube"', e.query.hd && (d += ' res="hd"'), e.fragment && (g = F(e.fragment), g.t && (d += ' start="' + parseInt(g.t, 10) + '"')), d += "]" + e.path.substr(1) + "[/video]"), g=!0;
                        if (!g)
                            switch (h) {
                            case "jpg":
                            case "gif":
                            case "png":
                                if (/(www\.|i\.)imgur.com/i.test(e.domain))
                                    switch (e =
                                    "", 5 < f.length && (e = f.substr(f.length - 1)), e) {
                                    case "s":
                                    case "l":
                                    case "t":
                                        d = "[url=" + (d.substr(0, d.lastIndexOf("/") + 1) + f.substr(0, f.length - 1) + "." + h) + "][img]" + d + "[/img][/url]";
                                        break;
                                    default:
                                        d = "[img]" + d + "[/img]"
                                    } else 
                                        d = "[img]" + d + "[/img]"
                            }
                    }
                    c.focus();
                    s(d, a, !1)
                }, 50);
                break;
            case 88:
                return v(), !0;
            case 89:
                return K(), a.preventDefault(), a.stopPropagation(), !1;
            case 90:
                return a.shiftKey ? K() : (h = Math.max(0, h - 1), J()), a.preventDefault(), a.stopPropagation(), !1
            }
            if (null !== d && (b = q(), null !== b)) {
                if (!a.shiftKey && b.start == b.end) {
                    e =
                    c[0].value;
                    g = b.start;
                    i = g - 1;
                    f = e.length;
                    for (g = {
                        start: g,
                        end: g
                    }; 0 <= i;) {
                        if (L(e.charCodeAt(i))) {
                            i++;
                            break
                        }
                        i--
                    }
                    for (g.start = i; i < f&&!L(e.charCodeAt(i));)
                        i++;
                    g.end = i;
                    b = g
                }
                a.preventDefault();
                a.stopPropagation();
                s(d, b, !0);
                return !1
            }
        } else {
            d = null;
            switch (a.keyCode) {
            case 13:
                !a.shiftKey && M("list") && (d = "\n[*]");
                break;
            case 9:
                M("code") && (d = "\t")
            }
            if (null !== d)
                return s(d, null, !1), a.preventDefault(), a.stopPropagation(), !1;
            v()
        }
        return !0
    }, p = function() {
        var b = l.find("div.character-count"), d = c[0].value.length;
        C = d;
        if (0 < d && 5E4 >= d &&
        b.hasClass("over"))
            b.removeClass("over"), a('input.bginput[type="submit"]').attr("disabled", !1);
        else if (5E4 < d || 0 === d)
            b.addClass("over"), a('input.bginput[type="submit"]').attr("disabled", !0);
        B ? d = Math.round(1E5 * Math.random()) - 5E4 : 3E4 < d&&!b.is(":visible") && b.show();
        b.text(d + " / 50000")
    };
    a(f).ready(function() {
        r=!!a('form[action="newthread.php"]').length;
        y=!!a('form[action="newreply.php"]').length;
        G=!!a('form[action="editpost.php"]').length;
        B=!!a("body.forum_26").length;
        if (r || y || G) {
            c = a('textarea[name="message"]');
            l = a('<div class="post-wrapper"></div>');
            c.before(l);
            l.append('<div class="postinfo"><div class="save-state">New reply</div><div class="character-count">0 / 50000</div></div>');
            l.prepend(c);
            c.keyup(p);
            c.change(function() {
                v();
                p()
            });
            setInterval(function() {
                C != c[0].value.length && (v(), p())
            }, 1E3);
            B && l.find("div.character-count").show();
            void 0 !== c[0].selectionStart && void 0 !== c[0].selectionEnd ? x = "new" : f.selection && (x = "ie legacy");
            N&&!r && (a(f).delegate('td.postlinks > ul.postbuttons > li > a[href^="newreply.php"]',
            "click", O), m = a("div#content > div.standard"), m.length || (m = a('<div class="standard"><h2>Post Preview:</h2><div class="inner postbody"></div></div>'), m.css("width", "100%"), m.hide(), a("div#content > div.breadcrumbs").after(m)));
            if (A) {
                n = a('input[name="threadid"]').val();
                t = a('input[name="forumid"]').val();
                w = c[0].value;
                var d = null;
                r ? d = D("forum-" + t) : y && (d = D("reply-" + n));
                if (null !== d && d.message != w) {
                    var h = l.find("div.save-state"), g = new Date(d.time);
                    h.html("<strong>Draft from: " + g.toLocaleDateString() + " " + g.toLocaleTimeString() +
                    "</strong>");
                    g = a('<a href="#">Append</a>');
                    g.click(function() {
                        s(d.message, null, !1);
                        return !1
                    });
                    h.append(g);
                    g = a('<a href="#">Replace</a>');
                    g.click(function() {
                        c[0].value = "";
                        s(d.message, null, !1);
                        u(d.length);
                        return !1
                    });
                    h.append(g);
                    r || (g = a('<a href="#">Preview</a>'), g.click(function() {
                        var b = d.message, e = {
                            action: a('input[name="action"]').val(),
                            threadid: a('input[name="threadid"]').val(),
                            formkey: a('input[name="formkey"]').val(),
                            form_cookie: a('input[name="form_cookie"]').val(),
                            preview: "Preview Reply",
                            message: b
                        };
                        a('form[action="newreply.php"] input[type="checkbox"]:checked').each(function(b, d) {
                            d = a(d);
                            e[d.attr("name")] = "yes"
                        });
                        a.post("newreply.php?json=1", e, function(b) {
                            var d = m.find("div.postbody");
                            d.empty();
                            d.html(b.preview);
                            m.show();
                            SA.timg.scan(d);
                            j.css({
                                top: c.offset().top
                            });
                            b = a.browser.webkit || a.browser.safari ? "body" : "html";
                            d = m.offset();
                            d.top < a(b).scrollTop() && a(b).animate({
                                scrollTop: d.top
                            }, 150)
                        });
                        return !1
                    }), h.append(g))
                }
            } else 
                l.find("div.save-state").text("Drafts not enabled in your browser");
            "new" == x &&
            (e.adv_post_disabled ? (b = null, c.keyup(function() {
                C != c[0].value.length && (v(), p())
            })) : (j = a("<textarea></textarea>"), j.css({
                position: "absolute",
                left: - 2E3,
                top: c.offset().top
            }), a("body").append(j), c.keydown(Q), u(c[0].value.length), z()));
            p();
            c.focus()
        }
    })
})(window, document, jQuery);
window.SA || (SA = {});
SA.thread = new function(e, f, a) {
    var g = /\?postid=(\d+)/, i = /#post(\d+)/, k = "html", c = function(b) {
        var c = a("#" + b);
        if (c.length || "top" == b) {
            if (a.browser.msie && 9 > parseInt(a.browser.version, 10))
                e.location.href = "#" + b;
            else {
                var d = c.length ? c.offset().top: 0;
                c.attr("id", "");
                e.location.href = "#" + b;
                c.attr("id", b);
                a(k).animate({
                    scrollTop: d
                }, 150)
            }
            return !1
        }
        return !0
    }, j = function() {
        var b = a(this).attr("href"), e = null;
        if (/^https?:\/\//.test(b)&&!/^https?:\/\/(.*\.)?forums\.somethingawful\./.test(b) ||
        /^\/?attachment\.php/.test(b))
            return !0;
        g.test(b) ? e = g : i.test(b) && (e = i);
        return null !== e ? (b = "post" + e.exec(b)[1], c(b)) : !0
    };
    a(f).ready(function() {
        var b = a('<div class="jump_top left">UP</div>'), h = a('<div class="jump_top right">UP</div>');
        a(f).delegate("td.postbody a", "click", j);
        SA.utils.isMobile ? (a(".bbc-spoiler").bind("touchmove", function(a) {
            a.stopPropagation();
            a.preventDefault()
        }), a(".bbc-spoiler").bind("touchstart", function(b) {
            b.target == this && (a(this).toggleClass("stay"), b.stopPropagation(), b.preventDefault())
        })) :
        (a(".bbc-spoiler").click(function(b) {
            b.target == this && a(this).toggleClass("stay")
        }), a(".bbc-spoiler").hover(function() {
            a(this).addClass("reveal")
        }, function() {
            a(this).removeClass("reveal")
        }));
        a("body.showpost").length || (false && 7 > parseInt(a.browser.version, 10) ? (h.show(), a("body").append(h)) : (a("body").append(b), a("body").append(h), a(f).mousemove(function(d) {
            var c = b.is(":visible"), f = h.is(":visible"), g = d.pageY > a(k).scrollTop() + a(e).height() - 100;
            g && 100 > d.pageX ? (f && h.hide(), c || b.show()) : (g && d.pageX >
            a(e).width() - 100 ? f || h.show() : f && h.hide(), c && b.hide())
        })));
        a("div.jump_top").click(function() {
            return c("top")
        });
        a('form.forum_jump select[name="forumid"]').change(function() {
            var b = a(this), c = b.val(), b = b.siblings('input[name="daysprune"]').val();
            if ( - 1 == c)
                return !1;
            e.location.href = "/forumdisplay.php?daysprune=" + b + "&forumid=" + c
        })
    });
    a(e).load(function() {
        if (a("body.showthread").length && e.adjust_page_position && e.location.hash) {
            var b = a(e.location.hash);
            b.length && (b = b.offset(), e.scrollTo(b.left, b.top))
        }
    })
}(window,
document, jQuery);
new function(e, f, a) {
    var g = 0, i = null, k = function() {
        clearInterval(g);
        a.get("/flag.php?forumid=26", function(a) {
            i.attr("title", 'This flag proudly brought to you by "' + a.username + '" on ' + a.created);
            i.attr("src", "http://fi.somethingawful.com/flags" + a.path + "?by=" + encodeURIComponent(a.username));
            g = setTimeout(k, 6E4)
        })
    };
    a(f).ready(function() {
        a(f).delegate("div.toggle_tags", "click", function() {
            a(this).parents("div#filter").eq(0).toggleClass("open")
        });
        a("div.pages select").change(function() {
            var b = a(this).attr("data-url");
            e.location.href = b + "&pagenumber=" + a(this).val()
        });
        if (a("body.forumdisplay, body.showthread").length) {
            var c = a("div.breadcrumbs > span:first-child");
            c.each(function(b, d) {
                var d = a(d), c = [];
                d.find("a").each(function(b, d) {
                    c.push(a(d).clone())
                });
                d.html(" &rsaquo; ");
                d.append(c.pop());
                var e = a(c.pop());
                d.prepend(e);
                if (c.length) {
                    var f = a("<span></span>");
                    f.append(c);
                    f.append(e.clone());
                    e.prepend(f)
                }
                e.addClass("up")
            });
            c.prepend('<a class="index" href="/" title="Forums index">&laquo;</a>&nbsp;')
        }
        a(f).delegate("div.thread_tags a.if",
        "click", function(b) {
            if (b.shiftKey) {
                b = /posticon=(\d+)/.exec(a(this).attr("href"));
                if (!b)
                    return !1;
                var b = b[1], d = SA.utils.qs(), c = null;
                if (d.posticon)
                    if ( - 1 == ("," + d.posticon).indexOf("," + b)) {
                        c = d.posticon.split(",");
                        for (c.push(b); 10 < c.length;)
                            c.shift();
                            d.posticon = c.join(",")
                    } else 
                        return !1;
                else 
                    d.posticon = b;
                e.location.href = "/forumdisplay.php?" + SA.utils.qs(d).replace(/%2c/ig, ",");
                return !1
            }
        });
        if (a("body.forum_26, body.forum_154").length)
            i = a(new Image), a("div#flag_container").append(i), k();
        else if (!SA.utils.isMobile && a("body.forum_25").length)
            c = a('<div id="gc_overlay"></div>'), a("body").append(c);
        else if (a("body.loginform").length) {
            var g = a("input.secure_login"), c = function() {
                var b = a("form.login_form"), c = "https://forums.somethingawful.com/account.php";
                g.is(":checked") ? a.cookie("secure_login", null) : (c = "/account.php", a.cookie("secure_login", "no"));
                b.attr("action", c)
            };
            g.click(c);
            "no" == a.cookie("secure_login") && (g.attr("checked", null), c())
        }
        var b = RegExp("^" + a("#loggedinusername").text().replace(/([.*+?^${}()|\[\]\/\\])/g,
        "\\$1") + "\\s+posted:$");
        a(".bbc-block h4").filter(function() {
            return b.test(a(this).text())
        }).map(function() {
            return a(this).closest(".bbc-block")[0]
        }).addClass("userquoted")
    })
}(window, document, jQuery);


