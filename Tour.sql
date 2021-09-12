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
	GiaTour float,
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
	SDT int,
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
	SoLuong int,
	ChiTiet nvarchar(100),
	TongGiaPT int,
	TongGiaKS int,
	TongGiaAU int,
	ChiPhiKhac int,
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
	CMND_Passport int,
	DiaChi nvarchar(50),
	SDT int,
	MaLoaiKhach int,
	HanVisa datetime,
	HanPassort datetime,
)

create table LoaiKhach(
	MaLoaiKhach int IDENTITY(1,1) primary key,
	TenLoaiKhach nvarchar(50),
)

create table PhuongTien(
	MaPT int IDENTITY(1,1) primary key,
	TenPT nvarchar(50),
	ChiPhi int,
)

create table DSPhuongTien(
	MaDSPT int IDENTITY(1,1) primary key,
	MaPT int,
	MaDoan int,
)

create table KhachSan(
	MaKS int IDENTITY(1,1) primary key,
	TenKS nvarchar(50),
	DiaChi nvarchar(50),
	SDT int,
	ChiPhi int,
	MaKhuVuc int,
)

create table DSKhachSan(
	MaDSKS int IDENTITY(1,1) primary key,
	MaKS int,
	MaDoan int,
)

Insert into LoaiKhach values('Domestic');
Insert into LoaiKhach values('Foreign');

Insert into LoaiTour values('Normal',1);
Insert into LoaiTour values('VIP',1.5);

Insert into KhuVuc values('An Giang');
Insert into KhuVuc values('TP HCM');

Insert into DiaDiem values(N'Núi Sam', 1);
Insert into DiaDiem values(N'Vạn Hương Mai',1);
Insert into DiaDiem values(N'Suối Tiên', 2);
Insert into DiaDiem values(N'Gò Vấp',2);

Insert into PhuongTien values(N'Xe Khách',100000);
Insert into PhuongTien values(N'Thuyền',200000);

Insert into KhachSan values(N'Hương Liên',N' Ấp An Thạnh, TT.An Phú, T.An Giang', 0345789987, 200000, 1);
Insert into KhachSan values(N'Hoa',N'Gò Vấp, Tp HCM', 0378654327, 150000, 2);

Insert into LoaiUser values('staff');
Insert into LoaiUser values('admin');

--password = admin (to base64 to md5)
Insert into Users values(2, N'Nguyễn Thiện Sua',20, 123456789, 0123456789, 'db69fc039dcbd2962cb4d28f5891aae1','123456789',2); 
Insert into Users values(1, N'Mai Long Thành',20, 123456789, 0369941633, 'db69fc039dcbd2962cb4d28f5891aae1','123456789',1);
Insert into Users values(3, N'admin',20, 123456789, 0123456789, 'db69fc039dcbd2962cb4d28f5891aae1','123456789',2);

Insert into NhanVien values(N'Nguyễn Hiếu Thành','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Nguyễn Hiếu Vũ','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Trần Thanh Nghĩa','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Trần Thanh Sua','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Trần Thiện Nghĩa','Nam', 0367941633, 123578910);

Insert into KhachHang values(N'Nguyễn Văn A','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null);
Insert into KhachHang values(N'Nguyễn Văn B','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null);
Insert into KhachHang values(N'Nguyễn Văn C','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null);
Insert into KhachHang values('Alex','Nam',18,1234567,N'An Phú An Giang',123456789,2,null,null);
Insert into KhachHang values('JohnSon','Nam',18,1234567,N'An Phú An Giang',123456789,2,null,null);

Insert into Tour values(N'An Phú - HCM',1,10000);
Insert into Tour values(N'An Phú - An Giang',1,20000);
Insert into Tour values(N'Đà Nẵng - Đà Lạt',2,10000);

insert into DSDiaDiem values(1,1);
insert into DSDiaDiem values(1,2);
insert into DSDiaDiem values(2,1);

insert into DoanDuLich values(N'Đoàn AG-HCM',1,'13/9/2021','15/9/2021',50,null,null, null,null,null);
insert into DoanDuLich values(N'Đoàn DN-DL',3,'13/9/2021','15/9/2021',50,null,null, null,null,null);
insert into DoanDuLich values(N'Đoàn Ap-AG',2,'13/9/2021','15/9/2021',50,null,null, null,null,null);

insert into DSPhuongTien values(1,1);
insert into DSPhuongTien values(1,2);
insert into DSPhuongTien values(2,1);

insert into DSKhachSan values(1,1);
insert into DSKhachSan values(1,2);
insert into DSKhachSan values(2,1);

insert into DSNhanVien values(1,1,N'Hướng dẫn viên');
insert into DSNhanVien values(1,2,N'Tài xế');
insert into DSNhanVien values(2,1,N'Phục vụ');










