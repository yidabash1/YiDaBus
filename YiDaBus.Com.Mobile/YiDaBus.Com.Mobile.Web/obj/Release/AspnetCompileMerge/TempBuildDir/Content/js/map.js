var lat1 = $("#mappoint").data("x");
var lng1 = $("#mappoint").data("y");
function getloaction(func) {
    try {
        var geolocation = new BMap.Geolocation();
        // 创建地理编码实例
        var myGeo = new BMap.Geocoder();
        geolocation.getCurrentPosition(function (r) {
            if (this.getStatus() == BMAP_STATUS_SUCCESS) {
                var pt = r.point;
                // 根据坐标得到地址描述
                myGeo.getLocation(pt, function (result) {
                    if (result) {
                        $("#locationdetails").html(result.addressComponents.district);
                        $("#locationdetails").data("x", pt.lat);
                        $("#locationdetails").data("y", pt.lng);
                        $("#mappoint").data("x", pt.lat);
                        $("#mappoint").data("y", pt.lng);
                        lat1 =pt.lng;
                        lng1 = pt.lat;
                        //if (func!="") {
                        //    func();
                        //}
                    }
                });
            }
        });
    } catch (err) {

    }
}
function getDistance(lat2, lng2) {

    //var pointA = new BMap.Point($("#locationdetails").data("x"), $("#locationdetails").data("y"));
    //var pointB = new BMap.Point(x2, y2);
    //return (map.getDistance(pointA, pointB)).toFixed(0);
    var radLat1 = rad(lat1);
    var radLat2 = rad(lat2);
    var a = radLat1 - radLat2;
    var b = rad(lng1) - rad(lng2);
    var s = 2 * Math.asin(Math.sqrt(Math.pow(Math.sin(a / 2), 2) + Math.cos(radLat1) * Math.cos(radLat2) * Math.pow(Math.sin(b / 2), 2)));
    s = s * 6378.137*100;
    // EARTH_RADIUS;
    s = Math.round(s * 10000) / 10000 || 0;
   // s = s > 1000 ? 1000 : s > 500 ? 500 : s < 200 ? 200 : s
    var enddistance = s.toFixed(0);
    return recount(enddistance);
}
function rad(d) {
    return d * Math.PI / 180.0;//经纬度转换成三角函数中度分表形式。
}
//重新计算返回值
//规则：1. 0m 返回 >200m 
//      2. <1000m, 返回 > 具体数字+m
//      3. >1000m, 重新计算为km,返回> 数字+km
//      4. >10000m, 返回 >10km
function recount(distance)
{
    var returndistance = '>2km';
    if (distance==0) {
        returndistance = returndistance;
    }
    else if (distance < 1000) {
        returndistance='>'+distance+'m'
    }
    else if(distance<9999)
    {
        returndistance = '>' + (distance / 1000).toFixed(1) + 'km'
    }
    else
    {
        returndistance = '>10km';
    }
    return returndistance;
}