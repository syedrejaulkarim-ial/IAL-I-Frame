﻿/*
 Highcharts JS v5.0.12 (2017-05-24)

 3D features for Highcharts JS

 @license: www.highcharts.com/license
*/
(function (A) { "object" === typeof module && module.exports ? module.exports = A : A(Highcharts) })(function (A) {
    (function (d) {
        var w = d.deg2rad, r = d.pick; d.perspective = function (t, x, y) {
            var m = x.options.chart.options3d, u = y ? x.inverted : !1, h = x.plotWidth / 2, v = x.plotHeight / 2, g = m.depth / 2, c = r(m.depth, 1) * r(m.viewDistance, 0), b = x.scale3d || 1, e = w * m.beta * (u ? -1 : 1), m = w * m.alpha * (u ? -1 : 1), a = Math.cos(m), f = Math.cos(-e), k = Math.sin(m), p = Math.sin(-e); y || (h += x.plotLeft, v += x.plotTop); return d.map(t, function (e) {
                var d, l; l = (u ? e.y : e.x) - h; var n = (u ?
                    e.x : e.y) - v, F = (e.z || 0) - g; d = f * l - p * F; e = -k * p * l + a * n - f * k * F; l = a * p * l + k * n + a * f * F; n = 0 < c && c < Number.POSITIVE_INFINITY ? c / (l + g + c) : 1; d = d * n * b + h; e = e * n * b + v; return { x: u ? e : d, y: u ? d : e, z: l * b + g }
            })
        }; d.pointCameraDistance = function (d, x) { var t = x.options.chart.options3d, m = x.plotWidth / 2; x = x.plotHeight / 2; t = r(t.depth, 1) * r(t.viewDistance, 0) + t.depth; return Math.sqrt(Math.pow(m - d.plotX, 2) + Math.pow(x - d.plotY, 2) + Math.pow(t - d.plotZ, 2)) }; d.shapeArea = function (d) {
            var t = 0, r, m; for (r = 0; r < d.length; r++)m = (r + 1) % d.length, t += d[r].x * d[m].y - d[m].x *
                d[r].y; return t / 2
        }; d.shapeArea3d = function (r, x, w) { return d.shapeArea(d.perspective(r, x, w)) }
    })(A); (function (d) {
        function w(a, b, e, d, c, f, k, g) {
            var B = [], n = f - c; return f > c && f - c > Math.PI / 2 + .0001 ? (B = B.concat(w(a, b, e, d, c, c + Math.PI / 2, k, g)), B = B.concat(w(a, b, e, d, c + Math.PI / 2, f, k, g))) : f < c && c - f > Math.PI / 2 + .0001 ? (B = B.concat(w(a, b, e, d, c, c - Math.PI / 2, k, g)), B = B.concat(w(a, b, e, d, c - Math.PI / 2, f, k, g))) : ["C", a + e * Math.cos(c) - e * l * n * Math.sin(c) + k, b + d * Math.sin(c) + d * l * n * Math.cos(c) + g, a + e * Math.cos(f) + e * l * n * Math.sin(f) + k, b + d * Math.sin(f) -
                d * l * n * Math.cos(f) + g, a + e * Math.cos(f) + k, b + d * Math.sin(f) + g]
        } var r = Math.cos, t = Math.PI, x = Math.sin, y = d.animObject, m = d.charts, u = d.color, h = d.defined, v = d.deg2rad, g = d.each, c = d.extend, b = d.inArray, e = d.map, a = d.merge, f = d.perspective, k = d.pick, p = d.SVGElement, z = d.SVGRenderer, q = d.wrap, l = 4 * (Math.sqrt(2) - 1) / 3 / (t / 2); z.prototype.toLinePath = function (a, b) { var e = []; g(a, function (a) { e.push("L", a.x, a.y) }); a.length && (e[0] = "M", b && e.push("Z")); return e }; z.prototype.toLineSegments = function (a) {
            var b = [], e = !0; g(a, function (a) {
                b.push(e ?
                    "M" : "L", a.x, a.y); e = !e
            }); return b
        }; z.prototype.face3d = function (a) {
            var b = this, e = this.createElement("path"); e.vertexes = []; e.insidePlotArea = !1; e.enabled = !0; q(e, "attr", function (a, e) {
                if ("object" === typeof e && (h(e.enabled) || h(e.vertexes) || h(e.insidePlotArea))) {
                this.enabled = k(e.enabled, this.enabled); this.vertexes = k(e.vertexes, this.vertexes); this.insidePlotArea = k(e.insidePlotArea, this.insidePlotArea); delete e.enabled; delete e.vertexes; delete e.insidePlotArea; var c = f(this.vertexes, m[b.chartIndex], this.insidePlotArea),
                    B = b.toLinePath(c, !0), c = d.shapeArea(c), c = this.enabled && 0 < c ? "visible" : "hidden"; e.d = B; e.visibility = c
                } return a.apply(this, [].slice.call(arguments, 1))
            }); q(e, "animate", function (a, e) {
                if ("object" === typeof e && (h(e.enabled) || h(e.vertexes) || h(e.insidePlotArea))) {
                this.enabled = k(e.enabled, this.enabled); this.vertexes = k(e.vertexes, this.vertexes); this.insidePlotArea = k(e.insidePlotArea, this.insidePlotArea); delete e.enabled; delete e.vertexes; delete e.insidePlotArea; var c = f(this.vertexes, m[b.chartIndex], this.insidePlotArea),
                    B = b.toLinePath(c, !0), c = d.shapeArea(c), c = this.enabled && 0 < c ? "visible" : "hidden"; e.d = B; this.attr("visibility", c)
                } return a.apply(this, [].slice.call(arguments, 1))
            }); return e.attr(a)
        }; z.prototype.polyhedron = function (a) {
            var b = this, e = this.g(), c = e.destroy; e.attr({ "stroke-linejoin": "round" }); e.faces = []; e.destroy = function () { for (var a = 0; a < e.faces.length; a++)e.faces[a].destroy(); return c.call(this) }; q(e, "attr", function (a, c, d, f, k) {
                if ("object" === typeof c && h(c.faces)) {
                    for (; e.faces.length > c.faces.length;)e.faces.pop().destroy();
                    for (; e.faces.length < c.faces.length;)e.faces.push(b.face3d().add(e)); for (var g = 0; g < c.faces.length; g++)e.faces[g].attr(c.faces[g], null, f, k); delete c.faces
                } return a.apply(this, [].slice.call(arguments, 1))
            }); q(e, "animate", function (a, c, d, f) {
                if (c && c.faces) { for (; e.faces.length > c.faces.length;)e.faces.pop().destroy(); for (; e.faces.length < c.faces.length;)e.faces.push(b.face3d().add(e)); for (var k = 0; k < c.faces.length; k++)e.faces[k].animate(c.faces[k], d, f); delete c.faces } return a.apply(this, [].slice.call(arguments,
                    1))
            }); return e.attr(a)
        }; z.prototype.cuboid = function (a) {
            var e = this.g(), b = e.destroy; a = this.cuboidPath(a); e.attr({ "stroke-linejoin": "round" }); e.front = this.path(a[0]).attr({ "class": "highcharts-3d-front" }).add(e); e.top = this.path(a[1]).attr({ "class": "highcharts-3d-top" }).add(e); e.side = this.path(a[2]).attr({ "class": "highcharts-3d-side" }).add(e); e.fillSetter = function (a) { this.front.attr({ fill: a }); this.top.attr({ fill: u(a).brighten(.1).get() }); this.side.attr({ fill: u(a).brighten(-.1).get() }); this.color = a; return this };
            e.opacitySetter = function (a) { this.front.attr({ opacity: a }); this.top.attr({ opacity: a }); this.side.attr({ opacity: a }); return this }; e.attr = function (a, e) { if ("string" === typeof a && "undefined" !== typeof e) { var b = a; a = {}; a[b] = e } if (a.shapeArgs || h(a.x)) a = this.renderer.cuboidPath(a.shapeArgs || a), this.front.attr({ d: a[0] }), this.top.attr({ d: a[1] }), this.side.attr({ d: a[2] }); else return d.SVGElement.prototype.attr.call(this, a); return this }; e.animate = function (a, e, b) {
            h(a.x) && h(a.y) ? (a = this.renderer.cuboidPath(a), this.front.animate({ d: a[0] },
                e, b), this.top.animate({ d: a[1] }, e, b), this.side.animate({ d: a[2] }, e, b), this.attr({ zIndex: -a[3] })) : a.opacity ? (this.front.animate(a, e, b), this.top.animate(a, e, b), this.side.animate(a, e, b)) : p.prototype.animate.call(this, a, e, b); return this
            }; e.destroy = function () { this.front.destroy(); this.top.destroy(); this.side.destroy(); return b.call(this) }; e.attr({ zIndex: -a[3] }); return e
        }; d.SVGRenderer.prototype.cuboidPath = function (a) {
            function b(a) { return r[a] } var c = a.x, k = a.y, g = a.z, n = a.height, v = a.width, l = a.depth, p = m[this.chartIndex],
                q, z, u = p.options.chart.options3d.alpha, h = 0, r = [{ x: c, y: k, z: g }, { x: c + v, y: k, z: g }, { x: c + v, y: k + n, z: g }, { x: c, y: k + n, z: g }, { x: c, y: k + n, z: g + l }, { x: c + v, y: k + n, z: g + l }, { x: c + v, y: k, z: g + l }, { x: c, y: k, z: g + l }], r = f(r, p, a.insidePlotArea); z = function (a, c) { var f = [[], -1]; a = e(a, b); c = e(c, b); 0 > d.shapeArea(a) ? f = [a, 0] : 0 > d.shapeArea(c) && (f = [c, 1]); return f }; q = z([3, 2, 1, 0], [7, 6, 5, 4]); a = q[0]; v = q[1]; q = z([1, 6, 7, 0], [4, 5, 2, 3]); n = q[0]; l = q[1]; q = z([1, 2, 5, 6], [0, 7, 4, 3]); z = q[0]; q = q[1]; 1 === q ? h += 1E4 * (1E3 - c) : q || (h += 1E4 * c); h += 10 * (!l || 0 <= u && 180 >= u || 360 > u &&
                    357.5 < u ? p.plotHeight - k : 10 + k); 1 === v ? h += 100 * g : v || (h += 100 * (1E3 - g)); h = -Math.round(h); return [this.toLinePath(a, !0), this.toLinePath(n, !0), this.toLinePath(z, !0), h]
        }; d.SVGRenderer.prototype.arc3d = function (e) {
            function d(e) { var c = !1, f = {}; e = a(e); for (var d in e) -1 !== b(d, l) && (f[d] = e[d], delete e[d], c = !0); return c ? f : !1 } var f = this.g(), n = f.renderer, l = "x y r innerR start end".split(" "); e = a(e); e.alpha *= v; e.beta *= v; f.top = n.path(); f.side1 = n.path(); f.side2 = n.path(); f.inn = n.path(); f.out = n.path(); f.onAdd = function () {
                var a =
                    f.parentGroup, e = f.attr("class"); f.top.add(f); g(["out", "inn", "side1", "side2"], function (b) { f[b].addClass(e + " highcharts-3d-side").add(a) })
            }; f.setPaths = function (a) { var e = f.renderer.arc3dPath(a), b = 100 * e.zTop; f.attribs = a; f.top.attr({ d: e.top, zIndex: e.zTop }); f.inn.attr({ d: e.inn, zIndex: e.zInn }); f.out.attr({ d: e.out, zIndex: e.zOut }); f.side1.attr({ d: e.side1, zIndex: e.zSide1 }); f.side2.attr({ d: e.side2, zIndex: e.zSide2 }); f.zIndex = b; f.attr({ zIndex: b }); a.center && (f.top.setRadialReference(a.center), delete a.center) };
            f.setPaths(e); f.fillSetter = function (a) { var e = u(a).brighten(-.1).get(); this.fill = a; this.side1.attr({ fill: e }); this.side2.attr({ fill: e }); this.inn.attr({ fill: e }); this.out.attr({ fill: e }); this.top.attr({ fill: a }); return this }; g(["opacity", "translateX", "translateY", "visibility"], function (a) { f[a + "Setter"] = function (a, e) { f[e] = a; g(["out", "inn", "side1", "side2", "top"], function (b) { f[b].attr(e, a) }) } }); q(f, "attr", function (a, e) {
                var b; "object" === typeof e && (b = d(e)) && (c(f.attribs, b), f.setPaths(f.attribs)); return a.apply(this,
                    [].slice.call(arguments, 1))
            }); q(f, "animate", function (e, b, c, f) { var g, n = this.attribs, v; delete b.center; delete b.z; delete b.depth; delete b.alpha; delete b.beta; v = y(k(c, this.renderer.globalAnimation)); v.duration && (g = d(b), b.dummy = 1, g && (v.step = function (e, b) { function c(a) { return n[a] + (k(g[a], n[a]) - n[a]) * b.pos } "dummy" === b.prop && b.elem.setPaths(a(n, { x: c("x"), y: c("y"), r: c("r"), innerR: c("innerR"), start: c("start"), end: c("end") })) }), c = v); return e.call(this, b, c, f) }); f.destroy = function () {
                this.top.destroy(); this.out.destroy();
                this.inn.destroy(); this.side1.destroy(); this.side2.destroy(); p.prototype.destroy.call(this)
            }; f.hide = function () { this.top.hide(); this.out.hide(); this.inn.hide(); this.side1.hide(); this.side2.hide() }; f.show = function () { this.top.show(); this.out.show(); this.inn.show(); this.side1.show(); this.side2.show() }; return f
        }; z.prototype.arc3dPath = function (a) {
            function e(a) { a %= 2 * Math.PI; a > Math.PI && (a = 2 * Math.PI - a); return a } var b = a.x, c = a.y, f = a.start, d = a.end - .00001, k = a.r, g = a.innerR, v = a.depth, l = a.alpha, p = a.beta, n = Math.cos(f),
                q = Math.sin(f); a = Math.cos(d); var h = Math.sin(d), z = k * Math.cos(p), k = k * Math.cos(l), m = g * Math.cos(p), u = g * Math.cos(l), g = v * Math.sin(p), y = v * Math.sin(l), v = ["M", b + z * n, c + k * q], v = v.concat(w(b, c, z, k, f, d, 0, 0)), v = v.concat(["L", b + m * a, c + u * h]), v = v.concat(w(b, c, m, u, d, f, 0, 0)), v = v.concat(["Z"]), A = 0 < p ? Math.PI / 2 : 0, p = 0 < l ? 0 : Math.PI / 2, A = f > -A ? f : d > -A ? -A : f, C = d < t - p ? d : f < t - p ? t - p : d, D = 2 * t - p, l = ["M", b + z * r(A), c + k * x(A)], l = l.concat(w(b, c, z, k, A, C, 0, 0)); d > D && f < D ? (l = l.concat(["L", b + z * r(C) + g, c + k * x(C) + y]), l = l.concat(w(b, c, z, k, C, D, g, y)), l = l.concat(["L",
                    b + z * r(D), c + k * x(D)]), l = l.concat(w(b, c, z, k, D, d, 0, 0)), l = l.concat(["L", b + z * r(d) + g, c + k * x(d) + y]), l = l.concat(w(b, c, z, k, d, D, g, y)), l = l.concat(["L", b + z * r(D), c + k * x(D)]), l = l.concat(w(b, c, z, k, D, C, 0, 0))) : d > t - p && f < t - p && (l = l.concat(["L", b + z * Math.cos(C) + g, c + k * Math.sin(C) + y]), l = l.concat(w(b, c, z, k, C, d, g, y)), l = l.concat(["L", b + z * Math.cos(d), c + k * Math.sin(d)]), l = l.concat(w(b, c, z, k, d, C, 0, 0))); l = l.concat(["L", b + z * Math.cos(C) + g, c + k * Math.sin(C) + y]); l = l.concat(w(b, c, z, k, C, A, g, y)); l = l.concat(["Z"]); p = ["M", b + m * n, c + u * q]; p = p.concat(w(b,
                        c, m, u, f, d, 0, 0)); p = p.concat(["L", b + m * Math.cos(d) + g, c + u * Math.sin(d) + y]); p = p.concat(w(b, c, m, u, d, f, g, y)); p = p.concat(["Z"]); n = ["M", b + z * n, c + k * q, "L", b + z * n + g, c + k * q + y, "L", b + m * n + g, c + u * q + y, "L", b + m * n, c + u * q, "Z"]; b = ["M", b + z * a, c + k * h, "L", b + z * a + g, c + k * h + y, "L", b + m * a + g, c + u * h + y, "L", b + m * a, c + u * h, "Z"]; h = Math.atan2(y, -g); c = Math.abs(d + h); a = Math.abs(f + h); f = Math.abs((f + d) / 2 + h); c = e(c); a = e(a); f = e(f); f *= 1E5; d = 1E5 * a; c *= 1E5; return {
                            top: v, zTop: 1E5 * Math.PI + 1, out: l, zOut: Math.max(f, d, c), inn: p, zInn: Math.max(f, d, c), side1: n, zSide1: .99 * c,
                            side2: b, zSide2: .99 * d
                        }
        }
    })(A); (function (d) {
        function w(d, g) {
            var c = d.plotLeft, b = d.plotWidth + c, e = d.plotTop, a = d.plotHeight + e, f = c + d.plotWidth / 2, k = e + d.plotHeight / 2, p = Number.MAX_VALUE, v = -Number.MAX_VALUE, q = Number.MAX_VALUE, l = -Number.MAX_VALUE, n, h = 1; n = [{ x: c, y: e, z: 0 }, { x: c, y: e, z: g }]; t([0, 1], function (a) { n.push({ x: b, y: n[a].y, z: n[a].z }) }); t([0, 1, 2, 3], function (e) { n.push({ x: n[e].x, y: a, z: n[e].z }) }); n = y(n, d, !1); t(n, function (a) { p = Math.min(p, a.x); v = Math.max(v, a.x); q = Math.min(q, a.y); l = Math.max(l, a.y) }); c > p && (h = Math.min(h,
                1 - Math.abs((c + f) / (p + f)) % 1)); b < v && (h = Math.min(h, (b - f) / (v - f))); e > q && (h = 0 > q ? Math.min(h, (e + k) / (-q + e + k)) : Math.min(h, 1 - (e + k) / (q + k) % 1)); a < l && (h = Math.min(h, Math.abs((a - k) / (l - k)))); return h
        } var r = d.Chart, t = d.each, x = d.merge, y = d.perspective, m = d.pick, u = d.wrap; r.prototype.is3d = function () { return this.options.chart.options3d && this.options.chart.options3d.enabled }; r.prototype.propsRequireDirtyBox.push("chart.options3d"); r.prototype.propsRequireUpdateSeries.push("chart.options3d"); d.wrap(d.Chart.prototype, "isInsidePlot",
            function (d) { return this.is3d() || d.apply(this, [].slice.call(arguments, 1)) }); var h = d.getOptions(); x(!0, h, { chart: { options3d: { enabled: !1, alpha: 0, beta: 0, depth: 100, fitToPlot: !0, viewDistance: 25, axisLabelPosition: "default", frame: { visible: "default", size: 1, bottom: {}, top: {}, left: {}, right: {}, back: {}, front: {} } } } }); u(r.prototype, "setClassName", function (d) { d.apply(this, [].slice.call(arguments, 1)); this.is3d() && (this.container.className += " highcharts-3d-chart") }); d.wrap(d.Chart.prototype, "setChartSize", function (d) {
                var g =
                    this.options.chart.options3d; d.apply(this, [].slice.call(arguments, 1)); if (this.is3d()) { var c = this.inverted, b = this.clipBox, e = this.margin; b[c ? "y" : "x"] = -(e[3] || 0); b[c ? "x" : "y"] = -(e[0] || 0); b[c ? "height" : "width"] = this.chartWidth + (e[3] || 0) + (e[1] || 0); b[c ? "width" : "height"] = this.chartHeight + (e[0] || 0) + (e[2] || 0); this.scale3d = 1; !0 === g.fitToPlot && (this.scale3d = w(this, g.depth)) }
            }); u(r.prototype, "redraw", function (d) {
                this.is3d() && (this.isDirtyBox = !0, this.frame3d = this.get3dFrame()); d.apply(this, [].slice.call(arguments,
                    1))
            }); u(r.prototype, "render", function (d) { this.is3d() && (this.frame3d = this.get3dFrame()); d.apply(this, [].slice.call(arguments, 1)) }); u(r.prototype, "renderSeries", function (d) { var g = this.series.length; if (this.is3d()) for (; g--;)d = this.series[g], d.translate(), d.render(); else d.call(this) }); u(r.prototype, "drawChartBox", function (v) {
                if (this.is3d()) {
                    var g = this.renderer, c = this.options.chart.options3d, b = this.get3dFrame(), e = this.plotLeft, a = this.plotLeft + this.plotWidth, f = this.plotTop, k = this.plotTop + this.plotHeight,
                    c = c.depth, p = e - (b.left.visible ? b.left.size : 0), h = a + (b.right.visible ? b.right.size : 0), q = f - (b.top.visible ? b.top.size : 0), l = k + (b.bottom.visible ? b.bottom.size : 0), n = 0 - (b.front.visible ? b.front.size : 0), m = c + (b.back.visible ? b.back.size : 0), u = this.hasRendered ? "animate" : "attr"; this.frame3d = b; this.frameShapes || (this.frameShapes = { bottom: g.polyhedron().add(), top: g.polyhedron().add(), left: g.polyhedron().add(), right: g.polyhedron().add(), back: g.polyhedron().add(), front: g.polyhedron().add() }); this.frameShapes.bottom[u]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-bottom",
                        zIndex: b.bottom.frontFacing ? -1E3 : 1E3, faces: [{ fill: d.color(b.bottom.color).brighten(.1).get(), vertexes: [{ x: p, y: l, z: n }, { x: h, y: l, z: n }, { x: h, y: l, z: m }, { x: p, y: l, z: m }], enabled: b.bottom.visible }, { fill: d.color(b.bottom.color).brighten(.1).get(), vertexes: [{ x: e, y: k, z: c }, { x: a, y: k, z: c }, { x: a, y: k, z: 0 }, { x: e, y: k, z: 0 }], enabled: b.bottom.visible }, { fill: d.color(b.bottom.color).brighten(-.1).get(), vertexes: [{ x: p, y: l, z: n }, { x: p, y: l, z: m }, { x: e, y: k, z: c }, { x: e, y: k, z: 0 }], enabled: b.bottom.visible && !b.left.visible }, {
                            fill: d.color(b.bottom.color).brighten(-.1).get(),
                            vertexes: [{ x: h, y: l, z: m }, { x: h, y: l, z: n }, { x: a, y: k, z: 0 }, { x: a, y: k, z: c }], enabled: b.bottom.visible && !b.right.visible
                        }, { fill: d.color(b.bottom.color).get(), vertexes: [{ x: h, y: l, z: n }, { x: p, y: l, z: n }, { x: e, y: k, z: 0 }, { x: a, y: k, z: 0 }], enabled: b.bottom.visible && !b.front.visible }, { fill: d.color(b.bottom.color).get(), vertexes: [{ x: p, y: l, z: m }, { x: h, y: l, z: m }, { x: a, y: k, z: c }, { x: e, y: k, z: c }], enabled: b.bottom.visible && !b.back.visible }]
                    }); this.frameShapes.top[u]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-top", zIndex: b.top.frontFacing ?
                            -1E3 : 1E3, faces: [{ fill: d.color(b.top.color).brighten(.1).get(), vertexes: [{ x: p, y: q, z: m }, { x: h, y: q, z: m }, { x: h, y: q, z: n }, { x: p, y: q, z: n }], enabled: b.top.visible }, { fill: d.color(b.top.color).brighten(.1).get(), vertexes: [{ x: e, y: f, z: 0 }, { x: a, y: f, z: 0 }, { x: a, y: f, z: c }, { x: e, y: f, z: c }], enabled: b.top.visible }, { fill: d.color(b.top.color).brighten(-.1).get(), vertexes: [{ x: p, y: q, z: m }, { x: p, y: q, z: n }, { x: e, y: f, z: 0 }, { x: e, y: f, z: c }], enabled: b.top.visible && !b.left.visible }, {
                                fill: d.color(b.top.color).brighten(-.1).get(), vertexes: [{
                                    x: h,
                                    y: q, z: n
                                }, { x: h, y: q, z: m }, { x: a, y: f, z: c }, { x: a, y: f, z: 0 }], enabled: b.top.visible && !b.right.visible
                            }, { fill: d.color(b.top.color).get(), vertexes: [{ x: p, y: q, z: n }, { x: h, y: q, z: n }, { x: a, y: f, z: 0 }, { x: e, y: f, z: 0 }], enabled: b.top.visible && !b.front.visible }, { fill: d.color(b.top.color).get(), vertexes: [{ x: h, y: q, z: m }, { x: p, y: q, z: m }, { x: e, y: f, z: c }, { x: a, y: f, z: c }], enabled: b.top.visible && !b.back.visible }]
                    }); this.frameShapes.left[u]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-left", zIndex: b.left.frontFacing ? -1E3 : 1E3, faces: [{
                            fill: d.color(b.left.color).brighten(.1).get(),
                            vertexes: [{ x: p, y: l, z: n }, { x: e, y: k, z: 0 }, { x: e, y: k, z: c }, { x: p, y: l, z: m }], enabled: b.left.visible && !b.bottom.visible
                        }, { fill: d.color(b.left.color).brighten(.1).get(), vertexes: [{ x: p, y: q, z: m }, { x: e, y: f, z: c }, { x: e, y: f, z: 0 }, { x: p, y: q, z: n }], enabled: b.left.visible && !b.top.visible }, { fill: d.color(b.left.color).brighten(-.1).get(), vertexes: [{ x: p, y: l, z: m }, { x: p, y: q, z: m }, { x: p, y: q, z: n }, { x: p, y: l, z: n }], enabled: b.left.visible }, {
                            fill: d.color(b.left.color).brighten(-.1).get(), vertexes: [{ x: e, y: f, z: c }, { x: e, y: k, z: c }, { x: e, y: k, z: 0 },
                            { x: e, y: f, z: 0 }], enabled: b.left.visible
                        }, { fill: d.color(b.left.color).get(), vertexes: [{ x: p, y: l, z: n }, { x: p, y: q, z: n }, { x: e, y: f, z: 0 }, { x: e, y: k, z: 0 }], enabled: b.left.visible && !b.front.visible }, { fill: d.color(b.left.color).get(), vertexes: [{ x: p, y: q, z: m }, { x: p, y: l, z: m }, { x: e, y: k, z: c }, { x: e, y: f, z: c }], enabled: b.left.visible && !b.back.visible }]
                    }); this.frameShapes.right[u]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-right", zIndex: b.right.frontFacing ? -1E3 : 1E3, faces: [{
                            fill: d.color(b.right.color).brighten(.1).get(),
                            vertexes: [{ x: h, y: l, z: m }, { x: a, y: k, z: c }, { x: a, y: k, z: 0 }, { x: h, y: l, z: n }], enabled: b.right.visible && !b.bottom.visible
                        }, { fill: d.color(b.right.color).brighten(.1).get(), vertexes: [{ x: h, y: q, z: n }, { x: a, y: f, z: 0 }, { x: a, y: f, z: c }, { x: h, y: q, z: m }], enabled: b.right.visible && !b.top.visible }, { fill: d.color(b.right.color).brighten(-.1).get(), vertexes: [{ x: a, y: f, z: 0 }, { x: a, y: k, z: 0 }, { x: a, y: k, z: c }, { x: a, y: f, z: c }], enabled: b.right.visible }, {
                            fill: d.color(b.right.color).brighten(-.1).get(), vertexes: [{ x: h, y: l, z: n }, { x: h, y: q, z: n }, {
                                x: h, y: q,
                                z: m
                            }, { x: h, y: l, z: m }], enabled: b.right.visible
                        }, { fill: d.color(b.right.color).get(), vertexes: [{ x: h, y: q, z: n }, { x: h, y: l, z: n }, { x: a, y: k, z: 0 }, { x: a, y: f, z: 0 }], enabled: b.right.visible && !b.front.visible }, { fill: d.color(b.right.color).get(), vertexes: [{ x: h, y: l, z: m }, { x: h, y: q, z: m }, { x: a, y: f, z: c }, { x: a, y: k, z: c }], enabled: b.right.visible && !b.back.visible }]
                    }); this.frameShapes.back[u]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-back", zIndex: b.back.frontFacing ? -1E3 : 1E3, faces: [{
                            fill: d.color(b.back.color).brighten(.1).get(),
                            vertexes: [{ x: h, y: l, z: m }, { x: p, y: l, z: m }, { x: e, y: k, z: c }, { x: a, y: k, z: c }], enabled: b.back.visible && !b.bottom.visible
                        }, { fill: d.color(b.back.color).brighten(.1).get(), vertexes: [{ x: p, y: q, z: m }, { x: h, y: q, z: m }, { x: a, y: f, z: c }, { x: e, y: f, z: c }], enabled: b.back.visible && !b.top.visible }, { fill: d.color(b.back.color).brighten(-.1).get(), vertexes: [{ x: p, y: l, z: m }, { x: p, y: q, z: m }, { x: e, y: f, z: c }, { x: e, y: k, z: c }], enabled: b.back.visible && !b.left.visible }, {
                            fill: d.color(b.back.color).brighten(-.1).get(), vertexes: [{ x: h, y: q, z: m }, {
                                x: h, y: l,
                                z: m
                            }, { x: a, y: k, z: c }, { x: a, y: f, z: c }], enabled: b.back.visible && !b.right.visible
                        }, { fill: d.color(b.back.color).get(), vertexes: [{ x: e, y: f, z: c }, { x: a, y: f, z: c }, { x: a, y: k, z: c }, { x: e, y: k, z: c }], enabled: b.back.visible }, { fill: d.color(b.back.color).get(), vertexes: [{ x: p, y: l, z: m }, { x: h, y: l, z: m }, { x: h, y: q, z: m }, { x: p, y: q, z: m }], enabled: b.back.visible }]
                    }); this.frameShapes.front[u]({
                        "class": "highcharts-3d-frame highcharts-3d-frame-front", zIndex: b.front.frontFacing ? -1E3 : 1E3, faces: [{
                            fill: d.color(b.front.color).brighten(.1).get(),
                            vertexes: [{ x: p, y: l, z: n }, { x: h, y: l, z: n }, { x: a, y: k, z: 0 }, { x: e, y: k, z: 0 }], enabled: b.front.visible && !b.bottom.visible
                        }, { fill: d.color(b.front.color).brighten(.1).get(), vertexes: [{ x: h, y: q, z: n }, { x: p, y: q, z: n }, { x: e, y: f, z: 0 }, { x: a, y: f, z: 0 }], enabled: b.front.visible && !b.top.visible }, { fill: d.color(b.front.color).brighten(-.1).get(), vertexes: [{ x: p, y: q, z: n }, { x: p, y: l, z: n }, { x: e, y: k, z: 0 }, { x: e, y: f, z: 0 }], enabled: b.front.visible && !b.left.visible }, {
                            fill: d.color(b.front.color).brighten(-.1).get(), vertexes: [{ x: h, y: l, z: n }, {
                                x: h,
                                y: q, z: n
                            }, { x: a, y: f, z: 0 }, { x: a, y: k, z: 0 }], enabled: b.front.visible && !b.right.visible
                        }, { fill: d.color(b.front.color).get(), vertexes: [{ x: a, y: f, z: 0 }, { x: e, y: f, z: 0 }, { x: e, y: k, z: 0 }, { x: a, y: k, z: 0 }], enabled: b.front.visible }, { fill: d.color(b.front.color).get(), vertexes: [{ x: h, y: l, z: n }, { x: p, y: l, z: n }, { x: p, y: q, z: n }, { x: h, y: q, z: n }], enabled: b.front.visible }]
                    })
                } return v.apply(this, [].slice.call(arguments, 1))
            }); r.prototype.retrieveStacks = function (d) {
                var g = this.series, c = {}, b, e = 1; t(this.series, function (a) {
                    b = m(a.options.stack,
                        d ? 0 : g.length - 1 - a.index); c[b] ? c[b].series.push(a) : (c[b] = { series: [a], position: e }, e++)
                }); c.totalStacks = e + 1; return c
            }; r.prototype.get3dFrame = function () {
                var h = this, g = h.options.chart.options3d, c = g.frame, b = h.plotLeft, e = h.plotLeft + h.plotWidth, a = h.plotTop, f = h.plotTop + h.plotHeight, k = g.depth, p = d.shapeArea3d([{ x: b, y: f, z: k }, { x: e, y: f, z: k }, { x: e, y: f, z: 0 }, { x: b, y: f, z: 0 }], h), u = d.shapeArea3d([{ x: b, y: a, z: 0 }, { x: e, y: a, z: 0 }, { x: e, y: a, z: k }, { x: b, y: a, z: k }], h), q = d.shapeArea3d([{ x: b, y: a, z: 0 }, { x: b, y: a, z: k }, { x: b, y: f, z: k }, {
                    x: b, y: f,
                    z: 0
                }], h), l = d.shapeArea3d([{ x: e, y: a, z: k }, { x: e, y: a, z: 0 }, { x: e, y: f, z: 0 }, { x: e, y: f, z: k }], h), n = d.shapeArea3d([{ x: b, y: f, z: 0 }, { x: e, y: f, z: 0 }, { x: e, y: a, z: 0 }, { x: b, y: a, z: 0 }], h), r = d.shapeArea3d([{ x: b, y: a, z: k }, { x: e, y: a, z: k }, { x: e, y: f, z: k }, { x: b, y: f, z: k }], h), x = !1, w = !1, A = !1, G = !1; t([].concat(h.xAxis, h.yAxis, h.zAxis), function (a) { a && (a.horiz ? a.opposite ? w = !0 : x = !0 : a.opposite ? G = !0 : A = !0) }); var E = function (a, e, b) {
                    for (var c = ["size", "color", "visible"], d = {}, f = 0; f < c.length; f++)for (var k = c[f], g = 0; g < a.length; g++)if ("object" === typeof a[g]) {
                        var h =
                            a[g][k]; if (void 0 !== h && null !== h) { d[k] = h; break }
                    } a = b; !0 === d.visible || !1 === d.visible ? a = d.visible : "auto" === d.visible && (a = 0 <= e); return { size: m(d.size, 1), color: m(d.color, "none"), frontFacing: 0 < e, visible: a }
                }, c = { bottom: E([c.bottom, c.top, c], p, x), top: E([c.top, c.bottom, c], u, w), left: E([c.left, c.right, c.side, c], q, A), right: E([c.right, c.left, c.side, c], l, G), back: E([c.back, c.front, c], r, !0), front: E([c.front, c.back, c], n, !1) }; "auto" === g.axisLabelPosition ? (l = function (a, e) {
                    return a.visible !== e.visible || a.visible && e.visible &&
                        a.frontFacing !== e.frontFacing
                }, g = [], l(c.left, c.front) && g.push({ y: (a + f) / 2, x: b, z: 0 }), l(c.left, c.back) && g.push({ y: (a + f) / 2, x: b, z: k }), l(c.right, c.front) && g.push({ y: (a + f) / 2, x: e, z: 0 }), l(c.right, c.back) && g.push({ y: (a + f) / 2, x: e, z: k }), p = [], l(c.bottom, c.front) && p.push({ x: (b + e) / 2, y: f, z: 0 }), l(c.bottom, c.back) && p.push({ x: (b + e) / 2, y: f, z: k }), u = [], l(c.top, c.front) && u.push({ x: (b + e) / 2, y: a, z: 0 }), l(c.top, c.back) && u.push({ x: (b + e) / 2, y: a, z: k }), q = [], l(c.bottom, c.left) && q.push({ z: (0 + k) / 2, y: f, x: b }), l(c.bottom, c.right) && q.push({
                    z: (0 +
                        k) / 2, y: f, x: e
                }), f = [], l(c.top, c.left) && f.push({ z: (0 + k) / 2, y: a, x: b }), l(c.top, c.right) && f.push({ z: (0 + k) / 2, y: a, x: e }), b = function (a, e, b) { if (0 === a.length) return null; if (1 === a.length) return a[0]; for (var c = 0, d = y(a, h, !1), f = 1; f < d.length; f++)b * d[f][e] > b * d[c][e] ? c = f : b * d[f][e] === b * d[c][e] && d[f].z < d[c].z && (c = f); return a[c] }, c.axes = { y: { left: b(g, "x", -1), right: b(g, "x", 1) }, x: { top: b(u, "y", -1), bottom: b(p, "y", 1) }, z: { top: b(f, "y", -1), bottom: b(q, "y", 1) } }) : c.axes = {
                    y: { left: { x: b, z: 0 }, right: { x: e, z: 0 } }, x: {
                        top: { y: a, z: 0 }, bottom: {
                            y: f,
                            z: 0
                        }
                    }, z: { top: { x: A ? e : b, y: a }, bottom: { x: A ? e : b, y: f } }
                }; return c
            }
    })(A); (function (d) {
        function w(e, a) {
            if (e.chart.is3d() && "colorAxis" !== e.coll) {
                var b = e.chart, c = b.frame3d, d = b.plotLeft, g = b.plotWidth + d, m = b.plotTop, b = b.plotHeight + m, l = 0, n = 0; a = e.swapZ({ x: a.x, y: a.y, z: 0 }); if (e.isZAxis) if (e.opposite) { if (null === c.axes.z.top) return {}; n = a.y - m; a.x = c.axes.z.top.x; a.y = c.axes.z.top.y } else { if (null === c.axes.z.bottom) return {}; n = a.y - b; a.x = c.axes.z.bottom.x; a.y = c.axes.z.bottom.y } else if (e.horiz) if (e.opposite) {
                    if (null === c.axes.x.top) return {};
                    n = a.y - m; a.y = c.axes.x.top.y; a.z = c.axes.x.top.z
                } else { if (null === c.axes.x.bottom) return {}; n = a.y - b; a.y = c.axes.x.bottom.y; a.z = c.axes.x.bottom.z } else if (e.opposite) { if (null === c.axes.y.right) return {}; l = a.x - g; a.x = c.axes.y.right.x; a.z = c.axes.y.right.z } else { if (null === c.axes.y.left) return {}; l = a.x - d; a.x = c.axes.y.left.x; a.z = c.axes.y.left.z } a = h([a], e.chart)[0]; a.x += l; a.y += n
            } return a
        } var r, t = d.Axis, x = d.Chart, y = d.each, m = d.extend, u = d.merge, h = d.perspective, v = d.pick, g = d.splat, c = d.Tick, b = d.wrap; b(t.prototype, "setOptions",
            function (e, a) { e.call(this, a); this.chart.is3d() && "colorAxis" !== this.coll && (e = this.options, e.tickWidth = v(e.tickWidth, 0), e.gridLineWidth = v(e.gridLineWidth, 1)) }); b(t.prototype, "getPlotLinePath", function (e) {
                var a = e.apply(this, [].slice.call(arguments, 1)); if (!this.chart.is3d() || "colorAxis" === this.coll || null === a) return a; var b = this.chart, c = b.options.chart.options3d, c = this.isZAxis ? b.plotWidth : c.depth, b = b.frame3d, a = [this.swapZ({ x: a[1], y: a[2], z: 0 }), this.swapZ({ x: a[1], y: a[2], z: c }), this.swapZ({ x: a[4], y: a[5], z: 0 }),
                this.swapZ({ x: a[4], y: a[5], z: c })], c = []; this.horiz ? (this.isZAxis ? (b.left.visible && c.push(a[0], a[2]), b.right.visible && c.push(a[1], a[3])) : (b.front.visible && c.push(a[0], a[2]), b.back.visible && c.push(a[1], a[3])), b.top.visible && c.push(a[0], a[1]), b.bottom.visible && c.push(a[2], a[3])) : (b.front.visible && c.push(a[0], a[2]), b.back.visible && c.push(a[1], a[3]), b.left.visible && c.push(a[0], a[1]), b.right.visible && c.push(a[2], a[3])); c = h(c, this.chart, !1); return this.chart.renderer.toLineSegments(c)
            }); b(t.prototype, "getLinePath",
                function (b) { return this.chart.is3d() ? [] : b.apply(this, [].slice.call(arguments, 1)) }); b(t.prototype, "getPlotBandPath", function (b) { if (!this.chart.is3d() || "colorAxis" === this.coll) return b.apply(this, [].slice.call(arguments, 1)); var a = arguments, e = a[2], c = [], a = this.getPlotLinePath(a[1]), e = this.getPlotLinePath(e); if (a && e) for (var d = 0; d < a.length; d += 6)c.push("M", a[d + 1], a[d + 2], "L", a[d + 4], a[d + 5], "L", e[d + 4], e[d + 5], "L", e[d + 1], e[d + 2], "Z"); return c }); b(c.prototype, "getMarkPath", function (b) {
                    var a = b.apply(this, [].slice.call(arguments,
                        1)), a = [w(this.axis, { x: a[1], y: a[2], z: 0 }), w(this.axis, { x: a[4], y: a[5], z: 0 })]; return this.axis.chart.renderer.toLineSegments(a)
                }); b(c.prototype, "getLabelPosition", function (b) { var a = b.apply(this, [].slice.call(arguments, 1)); return w(this.axis, a) }); d.wrap(t.prototype, "getTitlePosition", function (b) { var a = b.apply(this, [].slice.call(arguments, 1)); return w(this, a) }); b(t.prototype, "drawCrosshair", function (b) {
                    var a = arguments; this.chart.is3d() && a[2] && (a[2] = {
                        plotX: a[2].plotXold || a[2].plotX, plotY: a[2].plotYold ||
                            a[2].plotY
                    }); b.apply(this, [].slice.call(a, 1))
                }); b(t.prototype, "destroy", function (b) { y(["backFrame", "bottomFrame", "sideFrame"], function (a) { this[a] && (this[a] = this[a].destroy()) }, this); b.apply(this, [].slice.call(arguments, 1)) }); t.prototype.swapZ = function (b, a) { return this.isZAxis ? (a = a ? 0 : this.chart.plotLeft, { x: a + b.z, y: b.y, z: b.x - a }) : b }; r = d.ZAxis = function () { this.init.apply(this, arguments) }; m(r.prototype, t.prototype); m(r.prototype, {
                    isZAxis: !0, setOptions: function (b) {
                        b = u({ offset: 0, lineWidth: 0 }, b); t.prototype.setOptions.call(this,
                            b); this.coll = "zAxis"
                    }, setAxisSize: function () { t.prototype.setAxisSize.call(this); this.width = this.len = this.chart.options.chart.options3d.depth; this.right = this.chart.chartWidth - this.width - this.left }, getSeriesExtremes: function () {
                        var b = this, a = b.chart; b.hasVisibleSeries = !1; b.dataMin = b.dataMax = b.ignoreMinPadding = b.ignoreMaxPadding = null; b.buildStacks && b.buildStacks(); y(b.series, function (c) {
                            if (c.visible || !a.options.chart.ignoreHiddenSeries) b.hasVisibleSeries = !0, c = c.zData, c.length && (b.dataMin = Math.min(v(b.dataMin,
                                c[0]), Math.min.apply(null, c)), b.dataMax = Math.max(v(b.dataMax, c[0]), Math.max.apply(null, c)))
                        })
                    }
                }); b(x.prototype, "getAxes", function (b) { var a = this, c = this.options, c = c.zAxis = g(c.zAxis || {}); b.call(this); a.is3d() && (this.zAxis = [], y(c, function (b, c) { b.index = c; b.isX = !0; (new r(a, b)).setScale() })) })
    })(A); (function (d) {
        function w(d) { var g = d.apply(this, [].slice.call(arguments, 1)); this.chart.is3d() && (g.stroke = this.options.edgeColor || g.fill, g["stroke-width"] = x(this.options.edgeWidth, 1)); return g } var r = d.each, t = d.perspective,
            x = d.pick, y = d.Series, m = d.seriesTypes, u = d.inArray, h = d.svg; d = d.wrap; d(m.column.prototype, "translate", function (d) {
                d.apply(this, [].slice.call(arguments, 1)); if (this.chart.is3d()) {
                    var g = this, c = g.chart, b = g.options, e = b.depth || 25, a = g.borderWidth % 2 ? .5 : 0; if (c.inverted && !g.yAxis.reversed || !c.inverted && g.yAxis.reversed) a *= -1; var f = (b.stacking ? b.stack || 0 : g.index) * (e + (b.groupZPadding || 1)); !1 !== b.grouping && (f = 0); f += b.groupZPadding || 1; r(g.data, function (b) {
                        if (null !== b.y) {
                            var d = b.shapeArgs, h = b.tooltipPos, k; r([["x", "width"],
                            ["y", "height"]], function (b) { k = d[b[0]] - a; if (0 > k + d[b[1]] || k > g[b[0] + "Axis"].len) for (var c in d) d[c] = 0; 0 > k && (d[b[1]] += d[b[0]], d[b[0]] = 0); k + d[b[1]] > g[b[0] + "Axis"].len && (d[b[1]] = g[b[0] + "Axis"].len - d[b[0]]) }); b.shapeType = "cuboid"; d.z = f; d.depth = e; d.insidePlotArea = !0; h = t([{ x: h[0], y: h[1], z: f }], c, !0)[0]; b.tooltipPos = [h.x, h.y]
                        }
                    }); g.z = f
                }
            }); d(m.column.prototype, "animate", function (d) {
                if (this.chart.is3d()) {
                    var g = arguments[1], c = this.yAxis, b = this, e = this.yAxis.reversed; h && (g ? r(b.data, function (a) {
                    null !== a.y && (a.height =
                        a.shapeArgs.height, a.shapey = a.shapeArgs.y, a.shapeArgs.height = 1, e || (a.shapeArgs.y = a.stackY ? a.plotY + c.translate(a.stackY) : a.plotY + (a.negative ? -a.height : a.height)))
                    }) : (r(b.data, function (a) { null !== a.y && (a.shapeArgs.height = a.height, a.shapeArgs.y = a.shapey, a.graphic && a.graphic.animate(a.shapeArgs, b.options.animation)) }), this.drawDataLabels(), b.animate = null))
                } else d.apply(this, [].slice.call(arguments, 1))
            }); d(m.column.prototype, "plotGroup", function (d, g, c, b, e, a) {
                this.chart.is3d() && a && !this[g] && (this[g] = a, a.attr(this.getPlotBox()),
                    this[g].survive = !0); return d.apply(this, Array.prototype.slice.call(arguments, 1))
            }); d(m.column.prototype, "setVisible", function (d, g) { var c = this, b; c.chart.is3d() && r(c.data, function (e) { b = (e.visible = e.options.visible = g = void 0 === g ? !e.visible : g) ? "visible" : "hidden"; c.options.data[u(e, c.data)] = e.options; e.graphic && e.graphic.attr({ visibility: b }) }); d.apply(this, Array.prototype.slice.call(arguments, 1)) }); d(m.column.prototype, "init", function (d) {
                d.apply(this, [].slice.call(arguments, 1)); if (this.chart.is3d()) {
                    var g =
                        this.options, c = g.grouping, b = g.stacking, e = x(this.yAxis.options.reversedStacks, !0), a = 0; if (void 0 === c || c) { c = this.chart.retrieveStacks(b); a = g.stack || 0; for (b = 0; b < c[a].series.length && c[a].series[b] !== this; b++); a = 10 * (c.totalStacks - c[a].position) + (e ? b : -b); this.xAxis.reversed || (a = 10 * c.totalStacks - a) } g.zIndex = a
                }
            }); d(m.column.prototype, "pointAttribs", w); m.columnrange && (d(m.columnrange.prototype, "pointAttribs", w), m.columnrange.prototype.plotGroup = m.column.prototype.plotGroup, m.columnrange.prototype.setVisible =
                m.column.prototype.setVisible); d(y.prototype, "alignDataLabel", function (d) { if (this.chart.is3d() && ("column" === this.type || "columnrange" === this.type)) { var g = arguments[4], c = { x: g.x, y: g.y, z: this.z }, c = t([c], this.chart, !0)[0]; g.x = c.x; g.y = c.y } d.apply(this, [].slice.call(arguments, 1)) })
    })(A); (function (d) {
        var w = d.deg2rad, r = d.each, t = d.pick, x = d.seriesTypes, y = d.svg; d = d.wrap; d(x.pie.prototype, "translate", function (d) {
            d.apply(this, [].slice.call(arguments, 1)); if (this.chart.is3d()) {
                var m = this, h = m.options, v = h.depth || 0,
                g = m.chart.options.chart.options3d, c = g.alpha, b = g.beta, e = h.stacking ? (h.stack || 0) * v : m._i * v, e = e + v / 2; !1 !== h.grouping && (e = 0); r(m.data, function (a) { var d = a.shapeArgs; a.shapeType = "arc3d"; d.z = e; d.depth = .75 * v; d.alpha = c; d.beta = b; d.center = m.center; d = (d.end + d.start) / 2; a.slicedTranslation = { translateX: Math.round(Math.cos(d) * h.slicedOffset * Math.cos(c * w)), translateY: Math.round(Math.sin(d) * h.slicedOffset * Math.cos(c * w)) } })
            }
        }); d(x.pie.prototype.pointClass.prototype, "haloPath", function (d) {
            var m = arguments; return this.series.chart.is3d() ?
                [] : d.call(this, m[1])
        }); d(x.pie.prototype, "pointAttribs", function (d, u, h) { d = d.call(this, u, h); h = this.options; this.chart.is3d() && (d.stroke = h.edgeColor || u.color || this.color, d["stroke-width"] = t(h.edgeWidth, 1)); return d }); d(x.pie.prototype, "drawPoints", function (d) { d.apply(this, [].slice.call(arguments, 1)); this.chart.is3d() && r(this.points, function (d) { var h = d.graphic; if (h) h[d.y && d.visible ? "show" : "hide"]() }) }); d(x.pie.prototype, "drawDataLabels", function (d) {
            if (this.chart.is3d()) {
                var m = this.chart.options.chart.options3d;
                r(this.data, function (d) { var h = d.shapeArgs, g = h.r, c = (h.start + h.end) / 2, b = d.labelPos, e = -g * (1 - Math.cos((h.alpha || m.alpha) * w)) * Math.sin(c), a = g * (Math.cos((h.beta || m.beta) * w) - 1) * Math.cos(c); r([0, 2, 4], function (c) { b[c] += a; b[c + 1] += e }) })
            } d.apply(this, [].slice.call(arguments, 1))
        }); d(x.pie.prototype, "addPoint", function (d) { d.apply(this, [].slice.call(arguments, 1)); this.chart.is3d() && this.update(this.userOptions, !0) }); d(x.pie.prototype, "animate", function (d) {
            if (this.chart.is3d()) {
                var m = arguments[1], h = this.options.animation,
                r = this.center, g = this.group, c = this.markerGroup; y && (!0 === h && (h = {}), m ? (g.oldtranslateX = g.translateX, g.oldtranslateY = g.translateY, m = { translateX: r[0], translateY: r[1], scaleX: .001, scaleY: .001 }, g.attr(m), c && (c.attrSetters = g.attrSetters, c.attr(m))) : (m = { translateX: g.oldtranslateX, translateY: g.oldtranslateY, scaleX: 1, scaleY: 1 }, g.animate(m, h), c && c.animate(m, h), this.animate = null))
            } else d.apply(this, [].slice.call(arguments, 1))
        })
    })(A); (function (d) {
        var w = d.perspective, r = d.pick, t = d.Point, x = d.seriesTypes, y = d.wrap; y(x.scatter.prototype,
            "translate", function (d) { d.apply(this, [].slice.call(arguments, 1)); if (this.chart.is3d()) { var m = this.chart, h = r(this.zAxis, m.options.zAxis[0]), t = [], g, c, b; for (b = 0; b < this.data.length; b++)g = this.data[b], c = h.isLog && h.val2lin ? h.val2lin(g.z) : g.z, g.plotZ = h.translate(c), g.isInside = g.isInside ? c >= h.min && c <= h.max : !1, t.push({ x: g.plotX, y: g.plotY, z: g.plotZ }); m = w(t, m, !0); for (b = 0; b < this.data.length; b++)g = this.data[b], h = m[b], g.plotXold = g.plotX, g.plotYold = g.plotY, g.plotZold = g.plotZ, g.plotX = h.x, g.plotY = h.y, g.plotZ = h.z } });
        y(x.scatter.prototype, "init", function (d, r, h) {
            r.is3d() && (this.axisTypes = ["xAxis", "yAxis", "zAxis"], this.pointArrayMap = ["x", "y", "z"], this.parallelArrays = ["x", "y", "z"], this.directTouch = !0); d = d.apply(this, [r, h]); this.chart.is3d() && (this.tooltipOptions.pointFormat = this.userOptions.tooltip ? this.userOptions.tooltip.pointFormat || "x: \x3cb\x3e{point.x}\x3c/b\x3e\x3cbr/\x3ey: \x3cb\x3e{point.y}\x3c/b\x3e\x3cbr/\x3ez: \x3cb\x3e{point.z}\x3c/b\x3e\x3cbr/\x3e" : "x: \x3cb\x3e{point.x}\x3c/b\x3e\x3cbr/\x3ey: \x3cb\x3e{point.y}\x3c/b\x3e\x3cbr/\x3ez: \x3cb\x3e{point.z}\x3c/b\x3e\x3cbr/\x3e");
            return d
        }); y(x.scatter.prototype, "pointAttribs", function (m, r) { var h = m.apply(this, [].slice.call(arguments, 1)); this.chart.is3d() && r && (h.zIndex = d.pointCameraDistance(r, this.chart)); return h }); y(t.prototype, "applyOptions", function (d) { var m = d.apply(this, [].slice.call(arguments, 1)); this.series.chart.is3d() && void 0 === m.z && (m.z = 0); return m })
    })(A); (function (d) {
        var w = d.Axis, r = d.SVGRenderer, t = d.VMLRenderer; t && (d.setOptions({ animate: !1 }), t.prototype.face3d = r.prototype.face3d, t.prototype.polyhedron = r.prototype.polyhedron,
            t.prototype.cuboid = r.prototype.cuboid, t.prototype.cuboidPath = r.prototype.cuboidPath, t.prototype.toLinePath = r.prototype.toLinePath, t.prototype.toLineSegments = r.prototype.toLineSegments, t.prototype.createElement3D = r.prototype.createElement3D, t.prototype.arc3d = function (d) { d = r.prototype.arc3d.call(this, d); d.css({ zIndex: d.zIndex }); return d }, d.VMLRenderer.prototype.arc3dPath = d.SVGRenderer.prototype.arc3dPath, d.wrap(w.prototype, "render", function (d) {
                d.apply(this, [].slice.call(arguments, 1)); this.sideFrame &&
                    (this.sideFrame.css({ zIndex: 0 }), this.sideFrame.front.attr({ fill: this.sideFrame.color })); this.bottomFrame && (this.bottomFrame.css({ zIndex: 1 }), this.bottomFrame.front.attr({ fill: this.bottomFrame.color })); this.backFrame && (this.backFrame.css({ zIndex: 0 }), this.backFrame.front.attr({ fill: this.backFrame.color }))
            }))
    })(A)
});