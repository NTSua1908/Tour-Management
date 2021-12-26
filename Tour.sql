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
Insert into KhuVuc values(N'TP Hồ Chí Minh');
Insert into KhuVuc values(N'TP Hà Nội');
Insert into KhuVuc values(N'Đà Nẵng');
Insert into KhuVuc values(N'Bắc Ninh');
Insert into KhuVuc values(N'Đồng Tháp');
Insert into KhuVuc values(N'Kiên Giang');
Insert into KhuVuc values(N'Bình Dương');
Insert into KhuVuc values(N'Đồng Nai');

Insert into DiaDiem values(N'Núi Sam', 1);
Insert into DiaDiem values(N'Vạn Hương Mai',1);
Insert into DiaDiem values(N'Suối Tiên', 2);
Insert into DiaDiem values(N'Gò Vấp',2);
Insert into DiaDiem values(N'Cầu Vàng',4);
Insert into DiaDiem values(N'Hồ Gươm',3);
Insert into DiaDiem values(N'Chùa Dâu',5);
Insert into DiaDiem values(N'Đồng Sen Tháp Mười',6);
Insert into DiaDiem values(N'Phú Quốc',7);
Insert into DiaDiem values(N'Khu du lịch Đại Nam',8);
Insert into DiaDiem values(N'Quốc gia Nam Cát Tiên',9);


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
Insert into PhuongTien values('Vinashin', N'Thuyền', '123456789',20000000);

Insert into KhachSan values(N'Hương Liên', N'Mai Long Thành', N' Ấp An Thạnh, TT.An Phú, T.An Giang', 0345789987, 200000, 1);
Insert into KhachSan values(N'Hoa', N'Võ Thành Phát',N'Gò Vấp, Tp HCM', 0378654327, 150000, 2);
Insert into KhachSan values(N'Silverland Sakyo Hotel & Spa', N'Nguyễn Thiện Sua',N'10A Lê Thánh Tôn, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh', 0378654327, 150000, 2);
Insert into KhachSan values(N'Cochin Zen', N'Mai Long Thành',N'46 Thủ Khoa Huân, Street, Quận 1, Thành phố Hồ Chí Minh ', 0378654327, 150000, 2);
Insert into KhachSan values(N'The Reverie Sài Gòn', N'Võ Thành Phát',N'22-36 Nguyễn Huệ, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh', 0378654327, 150000, 2);
Insert into KhachSan values(N'Khách sạn Park Hyatt Sài Gòn', N'Nguyễn Thiện Sua',N'2 Lam Son Square, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh', 0378654327, 150000, 2);
Insert into KhachSan values(N'Sheraton Sài Gòn', N'Mai Long Thành',N'88 Đồng Khởi, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh', 0378654327, 150000, 2);
Insert into KhachSan values(N'Rex Sài Gòn', N'Võ Thành Phát',N'141 Nguyễn Huệ, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh', 0378654327, 150000, 2);

Insert into LoaiUser values('staff');
Insert into LoaiUser values('admin');

--password = admin (to base64 to md5)
Insert into Users values(2, N'Nguyễn Thiện Sua', 20, 123456789, 0123456789, 'ntsua','db69fc039dcbd2962cb4d28f5891aae1',2); 
Insert into Users values(1, N'Mai Long Thành', 20, 123456789, 0369941633, 'longthanh','db69fc039dcbd2962cb4d28f5891aae1',1);
Insert into Users values(1, N'Võ Thành Phát', 20, 123456789, 0369941633, 'thanhphat','db69fc039dcbd2962cb4d28f5891aae1',1);
Insert into Users values(3, N'BOSS', 20, 123456789, 0123456789, 'admin','db69fc039dcbd2962cb4d28f5891aae1',2);

Insert into NhanVien values(N'Nguyễn Hiếu Thành','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Nguyễn Hiếu Vũ','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Mai Long Vũ','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Trần Thanh Nghĩa','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Trần Thanh Sua','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Trần Thiện Nghĩa','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Trần Thiện Nghĩa','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Mai Thành Phát','Nam', 0367941633, 123578910);
Insert into NhanVien values(N'Võ Thiện Thành','Nam', 0367941633, 123578910);

Insert into KhachHang values(N'Lâm Anh Dũng','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Trần Đình Trung','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguyễn Huy Hoàng','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguyễn Mạnh Khôi','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Ngô Khôi Vĩ','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguyễn Minh Quang','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Lê Tùng Quân','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Hoàng Thanh Tùng','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Mai Thành Đạt','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguyễn Quang Khải','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguyễn Thế Bảo','Nam',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Lê Ái Nhi',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Lý Bảo Hân',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Võ Cẩm Nhung',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguyễn Dạ Thảo',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Huỳnh Diệp Anh',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguyễn Gia Hân',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Lâm Hạ Vy',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Trần Kỳ Anh',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Bùi Lan Khuê"',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Lâm Lệ Băng',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Trần Linh San',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Huỳnh Mai Vy',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Trương Minh Uyên',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguyễn Minh Trang',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Bùi Đoàn Như Quỳnh',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguyễn Ngân Anh',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Nguyễn Ngọc Dung',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Dương Ngọc Vy',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Lê Nhật Linh',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Lâm Quế Anh',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values(N'Mai Trúc Ly',N'Nữ',18,1234567,N'An Phú An Giang',123456789,1,null,null, 0);
Insert into KhachHang values('Alex','Nam',18,1234567,N'An Phú An Giang',123456789,2,'20/12/2022','20/12/2022', 0);
Insert into KhachHang values('JohnSon','Nam',18,1234567,N'An Phú An Giang',123456789,2,'20/12/2022','20/12/2022', 0);

Insert into Tour values(N'An Phú - HCM',1,1000000);
Insert into Tour values(N'An Phú - An Giang',1,2000000);
Insert into Tour values(N'Đà Nẵng - HCM',2,1000000);
Insert into Tour values(N'HCM - Đà Lạt',2,1000000);
Insert into Tour values(N'An Giang - Đà Lạt',2,1000000);

insert into DoanDuLich values(N'Đoàn AG-HCM',1,'13/9/2021','15/9/2021',50, 50,'', 10000000, 10000000, 20000000, 0, 0);
insert into DoanDuLich values(N'Đoàn DN-DL',3,'13/9/2021','15/9/2021',50, 35, '', 20000000, 10000000, 10000000, 0, 0);
insert into DoanDuLich values(N'Đoàn Ap-AG',2,'13/9/2021','15/9/2021',50, 48, '', 10000000, 10000000, 30000000, 0, 0);
insert into DoanDuLich values(N'Đoàn Ap-AG 2',2,'13/10/2021','15/10/2021',50, 48, '', 10000000, 10000000, 30000000, 0, 0);
insert into DoanDuLich values(N'Đoàn Ap-AG 3',2,'13/10/2021','15/10/2021', 50, 0, '', 0, 0, 0, 0, 0);

insert into DSDiaDiem values(1,1);
insert into DSDiaDiem values(1,2);
insert into DSDiaDiem values(2,1);

insert into DSPhuongTien values(1,1);
insert into DSPhuongTien values(1,2);
insert into DSPhuongTien values(2,1);

insert into DSKhachSan values(1, 3, 2, 1);
insert into DSKhachSan values(1, 4, 3, 2);
insert into DSKhachSan values(2, 5, 4, 1);

insert into DSNhanVien values(1,1,N'Hướng dẫn viên');
insert into DSNhanVien values(1,2,N'Tài xế');
insert into DSNhanVien values(2,1,N'Phục vụ');

