
$(document).ready(function () {
   
    loadOgrenciListesi();
    
   
    $("#ogrenciEkleForm").on("submit", function (e) {
        e.preventDefault();
        ekleOgrenci();
    });
    
   
    $(document).on("click", ".btn-duzenle", function () {
        const id = $(this).data("id");
        getOgrenciById(id);
    });
    
    
    $("#ogrenciGuncelleForm").on("submit", function (e) {
        e.preventDefault();
        guncelleOgrenci();
    });
    
   
    $(document).on("click", ".btn-sil", function () {
        const id = $(this).data("id");
        if (confirm("Bu öğrenciyi silmek istediğinize emin misiniz?")) {
            silOgrenci(id);
        }
    });
});


function loadOgrenciListesi() {
    $.ajax({
        url: "/Ogrenci/OgrenciListeAjax",
        type: "GET",
        contentType: "application/json",
        success: function (data) {
            let tableRows = "";
            $.each(data, function (index, ogrenci) {
                tableRows += `
                    <tr>
                        <td>${ogrenci.ogrenciid}</td>
                        <td>${ogrenci.ad}</td>
                        <td>${ogrenci.soyad}</td>
                        <td>
                            <button class="btn btn-sm btn-primary btn-duzenle" data-id="${ogrenci.ogrenciid}">Düzenle</button>
                            <button class="btn btn-sm btn-danger btn-sil" data-id="${ogrenci.ogrenciid}">Sil</button>
                        </td>
                    </tr>`;
            });
            $("#ogrenciListesiTbody").html(tableRows);
        },
        error: function (error) {
            console.log("Hata oluştu:", error);
            alert("Öğrenci listesi yüklenirken bir hata oluştu.");
        }
    });
}


function getOgrenciById(id) {
    $.ajax({
        url: `/Ogrenci/OgrenciGetirAjax?id=${id}`,
        type: "GET",
        contentType: "application/json",
        success: function (data) {
            
            $("#guncelleId").val(data.ogrenciid);
            $("#guncelleAd").val(data.ad);
            $("#guncelleSoyad").val(data.soyad);
            $("#guncelleModal").modal('show');
        },
        error: function (error) {
            console.log("Hata oluştu:", error);
            alert("Öğrenci bilgileri getirilirken bir hata oluştu.");
        }
    });
}

function ekleOgrenci() {
    const ogrenci = {
        ad: $("#ekleAd").val(),
        soyad: $("#ekleSoyad").val()
    };
    
    $.ajax({
        url: "/Ogrenci/OgrenciEkleAjax",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(ogrenci),
        success: function (response) {
            if (response.success) {
                alert(response.message);
                $("#ekleModal").modal('hide');
                $("#ogrenciEkleForm")[0].reset();
                loadOgrenciListesi(); 
            } else {
                alert(response.message);
            }
        },
        error: function (error) {
            console.log("Hata oluştu:", error);
            alert("Öğrenci eklenirken bir hata oluştu.");
        }
    });
}

function guncelleOgrenci() {
    const ogrenci = {
        ogrenciid: $("#guncelleId").val(),
        ad: $("#guncelleAd").val(),
        soyad: $("#guncelleSoyad").val()
    };
    
    $.ajax({
        url: "/Ogrenci/OgrenciGuncelleAjax",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(ogrenci),
        success: function (response) {
            if (response.success) {
                alert(response.message);
                $("#guncelleModal").modal('hide');
                loadOgrenciListesi(); 
            } else {
                alert(response.message);
            }
        },
        error: function (error) {
            console.log("Hata oluştu:", error);
            alert("Öğrenci güncellenirken bir hata oluştu.");
        }
    });
}


function silOgrenci(id) {
    $.ajax({
        url: "/Ogrenci/OgrenciSilAjax",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(id),
        success: function (response) {
            if (response.success) {
                alert(response.message);
                loadOgrenciListesi(); 
            } else {
                alert(response.message);
            }
        },
        error: function (error) {
            console.log("Hata oluştu:", error);
            alert("Öğrenci silinirken bir hata oluştu.");
        }
    });
}
