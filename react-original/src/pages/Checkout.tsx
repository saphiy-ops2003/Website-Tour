import { motion } from 'motion/react';
import { User, Mail, Phone, CreditCard, ShieldCheck, MapPin, CheckCircle, Info } from 'lucide-react';

export default function Checkout({ setView }: { setView: (v: string) => void }) {
  return (
    <div className="bg-slate-50 min-h-screen py-16 font-sans">
      <div className="max-w-7xl mx-auto px-6">
        
        {/* Progress */}
        <div className="flex items-center justify-center mb-16">
           <div className="flex items-center w-full max-w-2xl">
              <div className="flex flex-col items-center flex-1">
                 <div className="w-12 h-12 rounded-full bg-primary flex items-center justify-center text-white font-bold mb-3 shadow-lg shadow-primary/30">
                    <User className="w-5 h-5" />
                 </div>
                 <span className="text-[10px] font-bold text-primary uppercase tracking-widest text-center">Liên Hệ</span>
              </div>
              <div className="h-0.5 flex-1 bg-primary mb-8"></div>
              <div className="flex flex-col items-center flex-1">
                 <div className="w-12 h-12 rounded-full border-2 border-slate-200 bg-white flex items-center justify-center text-slate-300 font-bold mb-3">
                    <CreditCard className="w-5 h-5" />
                 </div>
                 <span className="text-[10px] font-bold text-slate-300 uppercase tracking-widest text-center">Thanh Toán</span>
              </div>
              <div className="h-0.5 flex-1 bg-slate-200 mb-8"></div>
              <div className="flex flex-col items-center flex-1">
                 <div className="w-12 h-12 rounded-full border-2 border-slate-200 bg-white flex items-center justify-center text-slate-300 font-bold mb-3">
                    <CheckCircle className="w-5 h-5" />
                 </div>
                 <span className="text-[10px] font-bold text-slate-300 uppercase tracking-widest text-center">Xác Nhận</span>
              </div>
           </div>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-12 gap-12">
          {/* Form */}
          <div className="lg:col-span-8 space-y-8">
            <section className="bg-white p-10 rounded-3xl shadow-sm border border-slate-100">
               <h2 className="text-2xl font-bold text-primary-dark mb-8 flex items-center gap-3">
                 <User className="text-primary w-6 h-6" /> Thông Tin Liên Hệ
               </h2>
               <div className="grid grid-cols-1 md:grid-cols-2 gap-6 font-medium">
                  <div className="space-y-2">
                     <label className="text-[10px] font-bold text-slate-400 uppercase tracking-widest ml-1">Họ và tên</label>
                     <input type="text" className="w-full p-4 rounded-xl border border-slate-200 focus:ring-2 focus:ring-primary outline-none transition-all" placeholder="Nguyễn Văn A" />
                  </div>
                  <div className="space-y-2">
                     <label className="text-[10px] font-bold text-slate-400 uppercase tracking-widest ml-1">Địa chỉ Email</label>
                     <input type="email" className="w-full p-4 rounded-xl border border-slate-200 focus:ring-2 focus:ring-primary outline-none transition-all" placeholder="nguyenvana@example.com" />
                  </div>
                  <div className="md:col-span-2 space-y-2">
                     <label className="text-[10px] font-bold text-slate-400 uppercase tracking-widest ml-1">Số điện thoại</label>
                     <div className="flex gap-3">
                        <select className="p-4 rounded-xl border border-slate-200 font-bold bg-slate-50">+84</select>
                        <input type="tel" className="flex-1 p-4 rounded-xl border border-slate-200 focus:ring-2 focus:ring-primary outline-none transition-all" placeholder="123 456 789" />
                     </div>
                  </div>
               </div>
            </section>

            <section className="bg-white p-10 rounded-3xl shadow-sm border border-slate-100">
               <h2 className="text-2xl font-bold text-primary-dark mb-8 flex items-center gap-3">
                 <CreditCard className="text-primary w-6 h-6" /> Phương Thức Thanh Toán
               </h2>
               <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-10">
                  <div className="p-6 border-2 border-primary bg-blue-50 rounded-2xl flex flex-col items-center gap-3 cursor-pointer">
                     <CreditCard className="text-primary" />
                     <span className="text-xs font-bold text-primary">Thẻ Tín Dụng</span>
                  </div>
                  <div className="p-6 border-2 border-slate-100 bg-slate-50 flex-col items-center gap-3 cursor-pointer opacity-50 grayscale flex rounded-2xl">
                     <div className="text-xs font-black text-pink-600">MoMo</div>
                  </div>
                  <div className="p-6 border-2 border-slate-100 bg-slate-50 flex-col items-center gap-3 cursor-pointer opacity-50 grayscale flex rounded-2xl">
                     <div className="text-xs font-black text-blue-500">ZaloPay</div>
                  </div>
                  <div className="p-6 border-2 border-slate-100 bg-slate-50 flex-col items-center gap-3 cursor-pointer opacity-50 grayscale flex rounded-2xl">
                     <span className="text-xs font-bold text-slate-400">Chuyển khoản</span>
                  </div>
               </div>
               
               <div className="space-y-6">
                  <div className="space-y-1">
                     <label className="text-[10px] font-bold text-slate-400 uppercase tracking-widest ml-1">Tên chủ thẻ</label>
                     <input type="text" className="w-full p-4 rounded-xl border border-slate-200 outline-none" placeholder="TÊN TRÊN THẺ" />
                  </div>
                  <div className="space-y-1">
                     <label className="text-[10px] font-bold text-slate-400 uppercase tracking-widest ml-1">Số thẻ</label>
                     <div className="relative">
                        <input type="text" className="w-full p-4 pr-12 rounded-xl border border-slate-200 outline-none" placeholder="0000 0000 0000 0000" />
                        <CreditCard className="absolute right-4 top-1/2 -translate-y-1/2 text-slate-300" />
                     </div>
                  </div>
                  <div className="grid grid-cols-2 gap-6">
                     <div className="space-y-1">
                        <label className="text-[10px] font-bold text-slate-400 uppercase tracking-widest ml-1">Ngày hết hạn</label>
                        <input type="text" className="w-full p-4 rounded-xl border border-slate-200 outline-none" placeholder="MM/YY" />
                     </div>
                     <div className="space-y-1">
                        <label className="text-[10px] font-bold text-slate-400 uppercase tracking-widest ml-1">CVV</label>
                        <input type="text" className="w-full p-4 rounded-xl border border-slate-200 outline-none" placeholder="***" />
                     </div>
                  </div>
               </div>

               <div className="mt-10 p-6 bg-green-50 border border-green-100 rounded-2xl flex items-start gap-4">
                  <ShieldCheck className="text-green-600 w-6 h-6 shrink-0" />
                  <div>
                    <h4 className="font-bold text-green-700 mb-1">Mã Hóa Bảo Mật</h4>
                    <p className="text-xs text-green-600/80 leading-relaxed">Giao dịch của bạn được bảo mật bằng mã hóa SSL 256-bit. Chúng tôi không lưu trữ thông tin thẻ của bạn.</p>
                  </div>
               </div>
            </section>
          </div>

          {/* Summary Sidebar */}
          <div className="lg:col-span-4">
            <aside className="sticky top-24 space-y-6">
               <div className="bg-white rounded-3xl overflow-hidden shadow-xl border border-slate-100">
                  <div className="h-40 relative">
                     <img src="https://lh3.googleusercontent.com/aida-public/AB6AXuCx3ak3Y4HCWIGz_VQEW3gcvoePQ3xoBq6Vtm7oEL5lpwd_ntyWLQWmUp5ybNZd8U3B_kG8caOkdnzqBuYPmhQCtBAbUQgiRriCPoR3aFcYDpAguCSFknBzvYKqnzio4M6mbDU-Ac0utD2jrM5DNBj9Ym9uOTpJFXLXsW76zRUuXSNcVEqwynUl5u6itODVZJ1M_yI56BG64BA4_n-IXYbEkKpn5ch98jiU0e7K_ZTFfb4fkvdn40UOF4RDZuk533BAqAiUn1Wohg" alt="Tour" className="w-full h-full object-cover" />
                  </div>
                  <div className="p-8">
                     <h3 className="text-xl font-bold text-primary-dark mb-4">Du Thuyền Sang Trọng Vịnh Hạ Long</h3>
                     <p className="text-slate-400 text-xs flex items-center gap-1 font-medium mb-6">
                        <MapPin className="w-3 h-3" /> Quảng Ninh, Việt Nam
                     </p>
                     
                     <div className="space-y-4 py-6 border-y border-slate-50 text-sm font-medium">
                        <div className="flex justify-between">
                           <span className="text-slate-400">Ngày</span>
                           <span className="text-slate-800">24 Tháng 10, 2024</span>
                        </div>
                        <div className="flex justify-between">
                           <span className="text-slate-400">Khách</span>
                           <span className="text-slate-800">2 Người lớn</span>
                        </div>
                        <div className="flex justify-between">
                           <span className="text-slate-400">Giá mỗi khách</span>
                           <span className="text-slate-800">$245.00</span>
                        </div>
                     </div>

                     <div className="space-y-4 py-6 border-b border-slate-50 text-sm font-medium mb-6">
                        <div className="flex justify-between">
                           <span className="text-slate-400">Tạm tính</span>
                           <span className="text-slate-800">$490.00</span>
                        </div>
                        <div className="flex justify-between">
                           <span className="text-slate-400">Thuế & Phí (10%)</span>
                           <span className="text-slate-800">$49.00</span>
                        </div>
                     </div>

                     <div className="flex justify-between items-center mb-8">
                        <span className="text-lg font-bold text-slate-800">Tổng Cộng</span>
                        <span className="text-3xl font-black text-accent">$539.00</span>
                     </div>

                     <button className="w-full py-5 bg-accent text-white font-bold rounded-2xl shadow-xl shadow-accent/20 hover:bg-accent-dark transition-all">
                        Thanh Toán & Xác Nhận
                     </button>
                  </div>
               </div>
               <div className="bg-blue-50/50 p-4 rounded-xl text-center border border-primary/5">
                  <p className="text-[10px] font-bold text-primary flex items-center justify-center gap-1 uppercase">
                    <Info className="w-3 h-3" /> Hủy miễn phí trước 22 Tháng 10, 2024
                  </p>
               </div>
            </aside>
          </div>
        </div>
      </div>
    </div>
  );
}
