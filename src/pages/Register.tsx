import { motion } from 'motion/react';
import { 
  User, 
  Mail as MailIcon, 
  PhoneCall, 
  Lock as LockIcon, 
  RotateCcw,
  ArrowRight,
  Star as StarIcon,
  MapPin
} from 'lucide-react';

export default function Register({ setView }: { setView: (v: string) => void }) {
  return (
    <div className="min-h-screen flex flex-col md:flex-row font-sans">
      {/* Left side: Hero */}
      <section className="hidden md:flex md:w-3/5 relative overflow-hidden bg-primary-dark">
        <img 
          src="https://lh3.googleusercontent.com/aida-public/AB6AXuAldUcBYDMtW5rg3NuLC-eDTEk9AIBe4TRAkE2RjSCjCLuvVnnZx4kRR1emcFaURqIn4hNGP2GLSNOeCXoPX3Iei5Ehf3x7Db_tVWFsxFaBW_nqaJsqDeEjBCoJWjmDfD5Lc4FBcNC77IvPbpqN4jC0nyokQ0xjsriOg7mUSnb3NJBET5epOha-I2zIJTrtkYncnqcyFiQkjXzeKQ9N39re_aDXdVqJKUuh9thV1MCwPJ66SJZfO40Vb6lr58LOMG4IPLZ9eKI3Sg" 
          alt="Vietnam Landscape" 
          className="absolute inset-0 w-full h-full object-cover"
        />
        <div className="absolute inset-0 bg-gradient-to-t from-primary-dark/80 via-transparent to-transparent"></div>
        <div className="relative z-10 p-20 flex flex-col justify-end h-full max-w-2xl">
          <div className="mb-8">
            <span className="text-white font-black text-2xl tracking-tight">VietExplore</span>
          </div>
          <motion.h1 
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            className="text-white text-5xl font-bold mb-6 leading-tight"
          >
            Khám phá vẻ đẹp tiềm ẩn của Việt Nam.
          </motion.h1>
          <p className="text-white/90 text-lg mb-10 leading-relaxed">
            Hành trình của bạn bắt đầu từ đây. Tham gia cộng đồng VietExplore để nhận những ưu đãi đặc quyền và cảm hứng du lịch mỗi ngày.
          </p>
          <div className="flex gap-6">
            <div className="flex items-center gap-4 bg-white/10 backdrop-blur-md p-4 rounded-xl">
              <StarIcon className="text-white fill-white w-6 h-6" />
              <div>
                <p className="text-white font-bold">500k+</p>
                <p className="text-white/70 text-xs uppercase tracking-tighter">Thành viên</p>
              </div>
            </div>
            <div className="flex items-center gap-4 bg-white/10 backdrop-blur-md p-4 rounded-xl">
              <MapPin className="text-white fill-white w-6 h-6" />
              <div>
                <p className="text-white font-bold">2000+</p>
                <p className="text-white/70 text-xs uppercase tracking-tighter">Điểm đến</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Right side: Form */}
      <section className="w-full md:w-2/5 flex flex-col justify-center items-center p-8 md:p-16 bg-white relative overflow-y-auto">
        <div className="w-full max-w-md z-10">
          <div className="mb-10">
            <div className="mb-4">
              <span className="text-2xl font-black text-primary-dark">VietExplore</span>
            </div>
            <h2 className="text-3xl font-bold text-primary-dark mb-2">Bắt đầu hành trình của bạn</h2>
            <p className="text-slate-500">Tạo tài khoản để mở khóa những trải nghiệm tuyệt vời nhất tại Việt Nam.</p>
          </div>

          <form className="space-y-4" onSubmit={(e) => e.preventDefault()}>
            <div className="space-y-1">
              <label className="block text-sm font-semibold text-slate-600 ml-1">Họ và tên</label>
              <div className="relative">
                <User className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 w-5 h-5" />
                <input 
                  type="text" 
                  className="w-full pl-11 pr-4 py-3 rounded-lg border border-slate-200 focus:ring-2 focus:ring-primary focus:border-primary transition-all outline-none" 
                  placeholder="Nguyễn Văn A" 
                />
              </div>
            </div>

            <div className="space-y-1">
              <label className="block text-sm font-semibold text-slate-600 ml-1">Email</label>
              <div className="relative">
                <MailIcon className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 w-5 h-5" />
                <input 
                  type="email" 
                  className="w-full pl-11 pr-4 py-3 rounded-lg border border-slate-200 focus:ring-2 focus:ring-primary focus:border-primary transition-all outline-none" 
                  placeholder="email@vietae.com" 
                />
              </div>
            </div>

            <div className="space-y-1">
              <label className="block text-sm font-semibold text-slate-600 ml-1">Số điện thoại</label>
              <div className="relative">
                <PhoneCall className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 w-5 h-5" />
                <input 
                  type="tel" 
                  className="w-full pl-11 pr-4 py-3 rounded-lg border border-slate-200 focus:ring-2 focus:ring-primary focus:border-primary transition-all outline-none" 
                  placeholder="090 123 4567" 
                />
              </div>
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div className="space-y-1">
                <label className="block text-sm font-semibold text-slate-600 ml-1">Mật khẩu</label>
                <div className="relative">
                  <LockIcon className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 w-5 h-5" />
                  <input 
                    type="password" 
                    className="w-full pl-11 pr-4 py-3 rounded-lg border border-slate-200 focus:ring-2 focus:ring-primary focus:border-primary transition-all outline-none" 
                    placeholder="••••••••" 
                  />
                </div>
              </div>
              <div className="space-y-1">
                <label className="block text-sm font-semibold text-slate-600 ml-1">Xác nhận</label>
                <div className="relative">
                  <RotateCcw className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 w-5 h-5" />
                  <input 
                    type="password" 
                    className="w-full pl-11 pr-4 py-3 rounded-lg border border-slate-200 focus:ring-2 focus:ring-primary focus:border-primary transition-all outline-none" 
                    placeholder="••••••••" 
                  />
                </div>
              </div>
            </div>

            <div className="flex items-start gap-3 py-2">
              <input type="checkbox" className="mt-1 w-5 h-5 rounded border-slate-300 text-primary focus:ring-primary" id="terms" />
              <label htmlFor="terms" className="text-sm text-slate-500 leading-tight">
                Tôi đồng ý với các <span className="text-primary font-semibold cursor-pointer">điều khoản dịch vụ</span> và <span className="text-primary font-semibold cursor-pointer">chính sách bảo mật</span> của VietExplore.
              </label>
            </div>

            <button 
              onClick={() => setView('home')}
              className="w-full py-4 bg-accent text-white font-bold rounded-lg shadow-lg hover:bg-accent-dark transition-all flex justify-center items-center gap-2"
            >
              Đăng ký ngay <ArrowRight className="w-5 h-5" />
            </button>
          </form>

          <div className="mt-8">
            <div className="relative flex items-center justify-center mb-6">
              <div className="flex-grow border-t border-slate-200"></div>
              <span className="flex-shrink mx-4 text-xs font-bold uppercase tracking-widest text-slate-400">Hoặc đăng ký bằng</span>
              <div className="flex-grow border-t border-slate-200"></div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <button className="flex items-center justify-center gap-2 py-3 px-4 rounded-lg border border-slate-200 hover:bg-slate-50 transition-colors font-semibold text-sm">
                <img src="https://lh3.googleusercontent.com/aida-public/AB6AXuBBoC736VSjCmvmLAjSNfCbCyDRw3DVyRLGfQnD91XBaeKjpYkROc9FJ5ZN7ugQzTZPG95NvT_pAPmOiiDayFhozlCgNKcnOLw9YF18uO0DBqj9kuxwLFhFb8RQ9gppLHQldsZTYN-1ct0zY264wwfvuHyI7DtFH4LxFMqtsJct3tiG3t0UKpAtNW_8XQK9UrVZZCj56duLotGrEG7f0AXCYwTKVyBH2etykbn1oOnij_euMErbZ3xIjRsl64n7u0SRuoLDOJBBeQ" className="w-5 h-5" alt="Google" />
                <span>Google</span>
              </button>
              <button className="flex items-center justify-center gap-2 py-3 px-4 rounded-lg bg-[#1877F2] text-white hover:opacity-90 transition-all font-semibold text-sm">
                 <span className="font-bold">f</span>
                <span>Facebook</span>
              </button>
            </div>
          </div>

          <div className="mt-12 text-center text-slate-600">
            Bạn đã có tài khoản? <span className="text-primary font-bold cursor-pointer hover:underline" onClick={() => setView('login')}>Đăng nhập ngay</span>
          </div>
        </div>
      </section>
    </div>
  );
}
