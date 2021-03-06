Create database Tour
use Tour
SET DATEFORMAT dmy
create table Users(
	ID int IDENTITY(1,1) primary key,
	Avatar int,
	HoTen nvarchar(50),
	Tuoi int,
	CMND nvarchar(20),
	SDT nvarchar(50),
	Taikhoan nvarchar(50),
	Password nvarchar(50),
	MaLoaiUser int,
)

create table LoaiUser(
	MaLoaiUser int IDENTITY(1,1) primary key,
	TenLoai nvarchar(20),
)

create table Tour(
	MaTour int IDENTITY(1,1) primary key,
	TenTour nvarchar(20),
	MaLoaiTour int,
	GiaTour money,
)

create table LoaiTour(
	MaLoaiTour int IDENTITY(1,1) primary key,
	TenLoaiTour nvarchar(20),
	HeSo float,
)

create table KhuVuc(
	MaKhuVuc int IDENTITY(1,1) primary key,
	TenKhuVuc nvarchar(50),
)

create table DiaDiem(
	MaDD int IDENTITY(1,1) primary key,
	TenDD nvarchar(50),
	MaKhuVuc int,
)

create table DSDiaDiem(
	MaDS int IDENTITY(1,1) primary key,
	MaDD int,
	MaTour int,
)

create table NhanVien(
	MaNV int IDENTITY(1,1) primary key,
	TenNV nvarchar(50),
	GioiTinh nvarchar(10),
	SDT varchar(50),
	CMND nvarchar(20),
)

create table DSNhanVien(
	MaDSNV int IDENTITY(1,1) primary key,
	MaNV int,
	MaDoan int,
	NhiemVu nvarchar(50),
)

create table DoanDuLich(
	MaDoan int IDENTITY(1,1) primary key,
	TenDoan nvarchar(50),
	MaTour int,
	NgayKhoiHanh datetime,
	NgayKetThuc datetime,
	SoLuongToiDa int,
	SoLuong int,
	ChiTiet nvarchar(max),
	TongGiaPT money,
	TongGiaKS money,
	TongGiaAU money,
	ChiPhiKhac money,
	SoLuongGiamGia int
)

create table KhachDuLich(
	MaKhachDL int IDENTITY(1,1) primary key,
	MaDoan int,
	MaKH int,
)

create table KhachHang(
	MaKH int IDENTITY(1,1) primary key,
	Hoten nvarchar(50),
	GioiTinh nvarchar(10),
	Tuoi int,
	CMND_Passport varchar(50),
	DiaChi nvarchar(100),
	SDT nvarchar(50),
	MaLoaiKhach int,
	HanVisa datetime,
	HanPassort datetime,
	SoLanDiTour int
)
create table KhachHangThanThiet(
	MaTT int IDENTITY(1,1) primary key,
	MaKH int
)

create table LoaiKhach(
	MaLoaiKhach int IDENTITY(1,1) primary key,
	TenLoaiKhach nvarchar(50),
)

create table PhuongTien(
	MaPT int IDENTITY(1,1) primary key,
	TenCongTy nvarchar(50),
	TenPT nvarchar(50),
	SDT varchar(50),
	ChiPhi money,
)

create table DSPhuongTien(
	MaDSPT int IDENTITY(1,1) primary key,
	MaPT int,
	MaDoan int,
)

create table KhachSan(
	MaKS int IDENTITY(1,1) primary key,
	TenKS nvarchar(50),
	NguoiDaiDien nvarchar(50),
	DiaChi nvarchar(100),
	SDT varchar(50),
	ChiPhi money,
	MaKhuVuc int,
)

create table DSKhachSan(
	MaDSKS int IDENTITY(1,1) primary key,
	MaKS int,
	SoNgay int,
	SoDem int,
	MaDoan int,
)

alter table Users add constraint fk_Users_LoaiUsers foreign key (MaLoaiUser) references LoaiUser (MaLoaiUser);
alter table Tour add constraint fk_Tour_LoaiTour foreign key (MaLoaiTour) references LoaiTour (MaLoaiTour);
alter table DSDiaDiem add constraint fk_DSDD_Tour foreign key (MaTour) references Tour (MaTour);
alter table DSDiaDiem add constraint fk_DSDD_DD foreign key (MaDD) references DiaDiem (MaDD);
alter table DiaDiem add constraint fk_DD_KhuVuc foreign key (MaKhuVuc) references KhuVuc (MaKhuVuc);
alter table DoanDuLich add constraint fk_DDL_Tour foreign key (MaTour) references Tour (MaTour);
alter table DSNhanVien add constraint fk_DSNV_DDL foreign key (MaDoan) references DoanDuLich (MaDoan);
alter table DSNhanVien add constraint fk_DSNV_NV foreign key (MaNV) references NhanVien(MaNV);
alter table KhachDuLich add constraint fk_KDL_DDL foreign key (MaDoan) references DoanDuLich (MaDoan);
alter table KhachDuLich add constraint fk_KDL_KH foreign key (MaKH) references KhachHang (MaKH);
alter table KhachHang add constraint fk_KH_LoaiKH foreign key (MaLoaiKhach) references LoaiKhach (MaLoaiKhach);
alter table KhachHangThanThiet add constraint fk_KHTT_KH foreign key (MaKH) references KhachHang (MaKH);
alter table DSPhuongTien add constraint fk_DSPT_DDL foreign key (MaDoan) references DoanDuLich (MaDoan);
alter table DSPhuongTien add constraint fk_DSPT_PT foreign key (MaPT) references PhuongTien (MaPT);
alter table DSKhachSan add constraint fk_DSKS_DDL foreign key (MaDoan) references DoanDuLich (MaDoan);
alter table DSKhachSan add constraint fk_DSKS_KS foreign key (MaKS) references KhachSan (MaKS);
alter table KhachSan add constraint fk_KS_KhuVuc foreign key (MaKhuVuc) references KhuVuc (MaKhuVuc);
ALTER TABLE DoanDuLich ADD CONSTRAINT check_SoLuong CHECK (SoLuong <= SoLuongToiDa)

Insert into LoaiKhach values('Domestic');
Insert into LoaiKhach values('Foreign');

Insert into LoaiTour values('Normal',1);
Insert into LoaiTour values('VIP',1.5);

Insert into KhuVuc values(N'An Giang');
Insert into KhuVuc values(N'TP H??? Ch?? Minh');
Insert into KhuVuc values(N'TP H?? N???i');
Insert into KhuVuc values(N'???? N???ng');
Insert into KhuVuc values(N'B???c Ninh');
Insert into KhuVuc values(N'?????ng Th??p');
Insert into KhuVuc values(N'Ki??n Giang');
Insert into KhuVuc values(N'B??nh D????ng');
Insert into KhuVuc values(N'?????ng Nai');

Insert into DiaDiem values(N'N??i Sam', 1);
Insert into DiaDiem values(N'V???n H????ng Mai',1);
Insert into DiaDiem values(N'Su???i Ti??n', 2);
Insert into DiaDiem values(N'G?? V???p',2);
Insert into DiaDiem values(N'C???u V??ng',4);
Insert into DiaDiem values(N'H??? G????m',3);
Insert into DiaDiem values(N'Ch??a D??u',5);
Insert into DiaDiem values(N'?????ng Sen Th??p M?????i',6);
Insert into DiaDiem values(N'Ph?? Qu???c',7);
Insert into DiaDiem values(N'Khu du l???ch ?????i Nam',8);
Insert into DiaDiem values(N'Qu???c gia Nam C??t Ti??n',9);


Insert into PhuongTien values('Toyota', N'Toyota Hiace', '123456789',1000000);
Insert into PhuongTien values('Ford', N'Ford Transit', '123456789',1000000);
Insert into PhuongTien values('Hyundai', N'Hyundai Solati', '123456789',1000000);
Insert into PhuongTien values('Mercedes', N'Mercedes Sprinter', '123456789',1000000);
Insert into PhuongTien values('Hyundai', N'Hyundai County', '123456789',2000000);
Insert into PhuongTien values('Isuzu', N'Isuzu Samco', '123456789',3000000);
Insert into PhuongTien values('Hyundai', N'Hynndai Aero Town', '123456789',4000000);
Insert into PhuongTien values('Thaco', N'Thaco Town', '123456789',10000000);
Insert into PhuongTien values('Hyundai', N'Hyundai Universe', '123456789',10000000);
Insert into PhuongTien values('Hyundai', N'Hyundai Aero Hi-class', '123456789',10000000);
Insert into PhuongTien values('Hyundai', N'Hyundai Space', '123456789',10000000);
Insert into PhuongTien values('Vinashin', N'Thuy???n', '123456789',20000000);

Insert into KhachSan values(N'H????ng Li??n', N'Mai Long Th??nh', N' ???p An Th???nh, TT.An Ph??, T.An Giang', 0345789987, 200000, 1);
Insert into KhachSan values(N'Hoa', N'V?? Th??nh Ph??t',N'G?? V???p, Tp HCM', 0378654327, 150000, 2);
Insert into KhachSan values(N'Silverland Sakyo Hotel & Spa', N'Nguy???n Thi???n Sua',N'10A L?? Th??nh T??n, B???n Ngh??, Qu???n 1, Th??nh ph??? H??? Ch?? Minh', 0378654327, 150000, 2);
Insert into KhachSan values(N'Cochin Zen', N'Mai Long Th??nh',N'46 Th??? Khoa Hu??n, Street, Qu???n 1, Th??nh ph??? H??? Ch?? Minh ', 0378654327, 150000, 2);
Insert into KhachSan values(N'The Reverie S??i G??n', N'V?? Th??nh Ph??t',N'22-36 Nguy????n Hu????, B???n Ngh??, Qu???n 1, Th??nh ph??? H??? Ch?? Minh', 0378654327, 150000, 2);
Insert into KhachSan values(N'Kh??ch s???n Park Hyatt S??i G??n', N'Nguy???n Thi???n Sua',N'2 Lam Son Square, B???n Ngh??, Qu???n 1, Th??nh ph??? H??? Ch?? Minh', 0378654327, 150000, 2);
Insert into KhachSan values(N'Sheraton S??i G??n', N'Mai Long Th??nh',N'88 ?????ng Kh???i, B???n Ngh??, Qu???n 1, Th??nh ph??? H??? Ch?? Minh', 0378654327, 150000, 2);
Insert into KhachSan values(N'Rex S??i G??n', N'V?? Th??nh Ph??t',N'141 Nguy????n Hu????, B???n Ngh??, Qu???n 1, Th??nh ph??? H??? Ch?? Minh', 0378654327, 150000, 2);

Insert into LoaiUser values('staff');
Insert into LoaiUser values('admin');

--password = admin (to base64 to md5)
Insert into Users values(2, N'Nguy???n Thi???n Sua', 20, 123456789, 0123456789, 'ntsua','db69fc039dcbd2962cb4d28f5891aae1',2); 
Insert into Users values(1, N'Mai Long Th??nh', 20, 123456789, 0369941633, 'longthanh','db69fc039dcbd2962cb4d28f5891aae1',1);
Insert into Users values(1, N'V?? Th??nh Ph??t', 20, 123456789, 0369941633, 'thanhphat','db69fc039dcbd2962cb4d28f5891aae1',1);
Insert into Users values(3, N'BOSS', 20, 123456789, 0123456789, 'admin','db69fc039dcbd2962cb4d28f5891aae1',2);

Insert into NhanVien values(N'Nguy???n Hi???u Th??nh','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Nguy???n Hi???u V??','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Mai Long V??','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Tr???n Thanh Ngh??a','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Tr???n Thanh Sua','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Tr???n Thi???n Ngh??a','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Tr???n Thi???n Ngh??a','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Mai Th??nh Ph??t','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'V?? Thi???n Th??nh','Nam', 0367941633, 123578910);

Insert into KhachHang values(N'L??m Anh D??ng','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Tr???n ????nh Trung','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguy???n Huy Ho??ng','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguy???n M???nh Kh??i','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Ng?? Kh??i V??','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguy???n Minh Quang','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'L?? T??ng Qu??n','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Ho??ng Thanh T??ng','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Mai Th??nh ?????t','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguy???n Quang Kh???i','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguy???n Th??? B???o','Nam',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'L?? ??i Nhi',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'L?? B???o H??n',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'V?? C???m Nhung',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguy???n D??? Th???o',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Hu???nh Di???p Anh',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguy???n Gia H??n',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'L??m H??? Vy',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Tr???n K??? Anh',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'B??i Lan Khu??"',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'L??m L??? B??ng',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Tr???n Linh San',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Hu???nh Mai Vy',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Tr????ng Minh Uy??n',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguy???n Minh Trang',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'B??i ??o??n Nh?? Qu???nh',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguy???n Ng??n Anh',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguy???n Ng???c Dung',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'D????ng Ng???c Vy',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'L?? Nh???t Linh',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'L??m Qu??? Anh',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Mai Tr??c Ly',N'N???',18,1234567,N'An Ph?? An Giang',123456789,1,null,null, 0);
Insert into KhachHang values('Alex','Nam',18,1234567,N'An Ph?? An Giang',123456789,2,'20/12/2022','20/12/2022', 0);
Insert into KhachHang values('JohnSon','Nam',18,1234567,N'An Ph?? An Giang',123456789,2,'20/12/2022','20/12/2022', 0);

Insert into Tour values(N'An Ph?? - HCM',1,1000000);
Insert into Tour values(N'An Ph?? - An Giang',1,2000000);
Insert into Tour values(N'???? N???ng - HCM',2,1000000);
Insert into Tour values(N'HCM - ???? L???t',2,1000000);
Insert into Tour values(N'An Giang - ???? L???t',2,1000000);

insert into DoanDuLich values(N'??o??n AG-HCM',1,'13/9/2021','15/9/2021',50, 50,'', 10000000, 10000000, 20000000, 0, 0);
insert into DoanDuLich values(N'??o??n DN-DL',3,'13/9/2021','15/9/2021',50, 35, '', 20000000, 10000000, 10000000, 0, 0);
insert into DoanDuLich values(N'??o??n Ap-AG',2,'13/9/2021','15/9/2021',50, 48, '', 10000000, 10000000, 30000000, 0, 0);
insert into DoanDuLich values(N'??o??n Ap-AG 2',2,'13/10/2021','15/10/2021',50, 48, '', 10000000, 10000000, 30000000, 0, 0);
insert into DoanDuLich values(N'??o??n Ap-AG 3',2,'13/10/2021','15/10/2021', 50, 0, '', 0, 0, 0, 0, 0);

insert into DSDiaDiem values(1,1);
insert into DSDiaDiem values(1,2);
insert into DSDiaDiem values(2,1);

insert into DSPhuongTien values(1,1);
insert into DSPhuongTien values(1,2);
insert into DSPhuongTien values(2,1);

insert into DSKhachSan values(1, 3, 2, 1);
insert into DSKhachSan values(1, 4, 3, 2);
insert into DSKhachSan values(2, 5, 4, 1);

insert into DSNhanVien values(1,1,N'H?????ng d???n vi??n');
insert into DSNhanVien values(1,2,N'T??i x???');
insert into DSNhanVien values(2,1,N'Ph???c v???');

drop database Tour

