"use strict";(()=>{var H=Object.defineProperty;var u=(g,v)=>H(g,"name",{value:v,configurable:!0});(globalThis.webpackChunk=globalThis.webpackChunk||[]).push([[9523,9753],{59753:(g,v,x)=>{x.d(v,{f:()=>G,on:()=>W});function d(){if(!(this instanceof d))return new d;this.size=0,this.uid=0,this.selectors=[],this.selectorObjects={},this.indexes=Object.create(this.indexes),this.activeIndexes=[]}u(d,"SelectorSet");var m=window.document.documentElement,P=m.matches||m.webkitMatchesSelector||m.mozMatchesSelector||m.oMatchesSelector||m.msMatchesSelector;d.prototype.matchesSelector=function(e,t){return P.call(e,t)},d.prototype.querySelectorAll=function(e,t){return t.querySelectorAll(e)},d.prototype.indexes=[];var b=/^#((?:[\w\u00c0-\uFFFF\-]|\\.)+)/g;d.prototype.indexes.push({name:"ID",selector:u(function(t){var r;if(r=t.match(b))return r[0].slice(1)},"matchIdSelector"),element:u(function(t){if(t.id)return[t.id]},"getElementId")});var k=/^\.((?:[\w\u00c0-\uFFFF\-]|\\.)+)/g;d.prototype.indexes.push({name:"CLASS",selector:u(function(t){var r;if(r=t.match(k))return r[0].slice(1)},"matchClassSelector"),element:u(function(t){var r=t.className;if(r){if(typeof r=="string")return r.split(/\s/);if(typeof r=="object"&&"baseVal"in r)return r.baseVal.split(/\s/)}},"getElementClassNames")});var w=/^((?:[\w\u00c0-\uFFFF\-]|\\.)+)/g;d.prototype.indexes.push({name:"TAG",selector:u(function(t){var r;if(r=t.match(w))return r[0].toUpperCase()},"matchTagSelector"),element:u(function(t){return[t.nodeName.toUpperCase()]},"getElementTagName")}),d.prototype.indexes.default={name:"UNIVERSAL",selector:function(){return!0},element:function(){return[!0]}};var S;typeof window.Map=="function"?S=window.Map:S=function(){function e(){this.map={}}return u(e,"Map"),e.prototype.get=function(t){return this.map[t+" "]},e.prototype.set=function(t,r){this.map[t+" "]=r},e}();var F=/((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^\[\]]*\]|['"][^'"]*['"]|[^\[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?((?:.|\r|\n)*)/g;function O(e,t){e=e.slice(0).concat(e.default);var r=e.length,i,n,o,s,a=t,c,f,l=[];do if(F.exec(""),(o=F.exec(a))&&(a=o[3],o[2]||!a)){for(i=0;i<r;i++)if(f=e[i],c=f.selector(o[1])){for(n=l.length,s=!1;n--;)if(l[n].index===f&&l[n].key===c){s=!0;break}s||l.push({index:f,key:c});break}}while(o);return l}u(O,"parseSelectorIndexes");function z(e,t){var r,i,n;for(r=0,i=e.length;r<i;r++)if(n=e[r],t.isPrototypeOf(n))return n}u(z,"findByPrototype"),d.prototype.logDefaultIndexUsed=function(){},d.prototype.add=function(e,t){var r,i,n,o,s,a,c,f,l=this.activeIndexes,p=this.selectors,h=this.selectorObjects;if(typeof e=="string"){for(r={id:this.uid++,selector:e,data:t},h[r.id]=r,c=O(this.indexes,e),i=0;i<c.length;i++)f=c[i],o=f.key,n=f.index,s=z(l,n),s||(s=Object.create(n),s.map=new S,l.push(s)),n===this.indexes.default&&this.logDefaultIndexUsed(r),a=s.map.get(o),a||(a=[],s.map.set(o,a)),a.push(r);this.size++,p.push(e)}},d.prototype.remove=function(e,t){if(typeof e=="string"){var r,i,n,o,s,a,c,f,l=this.activeIndexes,p=this.selectors=[],h=this.selectorObjects,y={},_=arguments.length===1;for(r=O(this.indexes,e),n=0;n<r.length;n++)for(i=r[n],o=l.length;o--;)if(a=l[o],i.index.isPrototypeOf(a)){if(c=a.map.get(i.key),c)for(s=c.length;s--;)f=c[s],f.selector===e&&(_||f.data===t)&&(c.splice(s,1),y[f.id]=!0);break}for(n in y)delete h[n],this.size--;for(n in h)p.push(h[n].selector)}};function C(e,t){return e.id-t.id}u(C,"sortById"),d.prototype.queryAll=function(e){if(!this.selectors.length)return[];var t={},r=[],i=this.querySelectorAll(this.selectors.join(", "),e),n,o,s,a,c,f,l,p;for(n=0,s=i.length;n<s;n++)for(c=i[n],f=this.matches(c),o=0,a=f.length;o<a;o++)p=f[o],t[p.id]?l=t[p.id]:(l={id:p.id,selector:p.selector,data:p.data,elements:[]},t[p.id]=l,r.push(l)),l.elements.push(c);return r.sort(C)},d.prototype.matches=function(e){if(!e)return[];var t,r,i,n,o,s,a,c,f,l,p,h=this.activeIndexes,y={},_=[];for(t=0,n=h.length;t<n;t++)if(a=h[t],c=a.element(e),c){for(r=0,o=c.length;r<o;r++)if(f=a.map.get(c[r]))for(i=0,s=f.length;i<s;i++)l=f[i],p=l.id,!y[p]&&this.matchesSelector(e,l.selector)&&(y[p]=!0,_.push(l))}return _.sort(C)};var j={},I={},T=new WeakMap,M=new WeakMap,E=new WeakMap,A=Object.getOwnPropertyDescriptor(Event.prototype,"currentTarget");function q(e,t,r){var i=e[t];return e[t]=function(){return r.apply(e,arguments),i.apply(e,arguments)},e}u(q,"before");function L(e,t,r){var i=[],n=t;do{if(n.nodeType!==1)break;var o=e.matches(n);if(o.length){var s={node:n,observers:o};r?i.unshift(s):i.push(s)}}while(n=n.parentElement);return i}u(L,"dist_matches");function U(){T.set(this,!0)}u(U,"trackPropagation");function R(){T.set(this,!0),M.set(this,!0)}u(R,"trackImmediate");function B(){return E.get(this)||null}u(B,"getCurrentTarget");function D(e,t){!A||Object.defineProperty(e,"currentTarget",{configurable:!0,enumerable:!0,get:t||A.get})}u(D,"defineCurrentTarget");function V(e){try{return e.eventPhase,!0}catch{return!1}}u(V,"canDispatch");function N(e){if(!!V(e)){var t=e.eventPhase===1?I:j,r=t[e.type];if(!!r){var i=L(r,e.target,e.eventPhase===1);if(!!i.length){q(e,"stopPropagation",U),q(e,"stopImmediatePropagation",R),D(e,B);for(var n=0,o=i.length;n<o&&!T.get(e);n++){var s=i[n];E.set(e,s.node);for(var a=0,c=s.observers.length;a<c&&!M.get(e);a++)s.observers[a].data.call(s.node,e)}E.delete(e),D(e)}}}}u(N,"dispatch");function W(e,t,r){var i=arguments.length>3&&arguments[3]!==void 0?arguments[3]:{},n=!!i.capture,o=n?I:j,s=o[e];s||(s=new d,o[e]=s,document.addEventListener(e,N,n)),s.add(t,r)}u(W,"on");function J(e,t,r){var i=arguments.length>3&&arguments[3]!==void 0?arguments[3]:{},n=!!i.capture,o=n?I:j,s=o[e];!s||(s.remove(t,r),!s.size&&(delete o[e],document.removeEventListener(e,N,n)))}u(J,"off");function G(e,t,r){return e.dispatchEvent(new CustomEvent(t,{bubbles:!0,cancelable:!0,detail:r}))}u(G,"fire")},97661:(g,v,x)=>{var d=x(59753);(0,d.on)("click",".js-convert-to-discussion-modal-open-button",function(m){const b=m.currentTarget.closest("details");if(b){const k=b.querySelector('.js-convert-to-discussion-category[aria-checked="true"] input');if(k){k.checked=!0;const w=b.querySelector(".js-convert-to-discussion-button");w&&w.removeAttribute("disabled")}}})}},g=>{var v=u(d=>g(g.s=d),"__webpack_exec__"),x=v(97661)}]);})();

//# sourceMappingURL=issue-to-discussion-conversions-8ca97c4a90a8.js.map