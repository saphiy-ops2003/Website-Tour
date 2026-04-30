import { motion } from 'motion/react';
import { Rocket, Eye, Star, MapPin, ArrowRight, Phone, MessageCircle } from 'lucide-react';

export default function Home({ setView }: { setView: (v: string) => void }) {
  return (
    <div className="font-sans">
      {/* Hero Section */}
      <section className="relative h-[650px] flex items-center justify-center overflow-hidden">
        <div className="absolute inset-0 z-0">
          <img 
            src="https://lh3.googleusercontent.com/aida-public/AB6AXuDaMglBYqPgTF8joy7vDWAjM86_b8K1qfmHY9JdwSasQ2CWwMDKTE4Yle9Iyd7-6d48UJyTYR0f_AOyu7OvddcOek58eZIE5b4IAzi3W1DpFh-avm8WieQcDKXnqUfCCucn3DU7W7-epn8xZsweGGfdJdwFWHEOmfTEvhBe44AZTbGE-rGuUfMMfDM6iXTv2dN_92q1DznBMEqHlPK3y495EO6jG7OZWcD03tc9l4H-kwR69ysrHSB2wEkkMASgt_A2AcpebwSXcA" 
            alt="Ha Long Bay" 
            className="w-full h-full object-cover"
          />
          <div className="absolute inset-0 bg-black/40"></div>
        </div>
        <div className="relative z-10 text-center px-6 max-w-4xl">
          <motion.h1 
            initial={{ opacity: 0, scale: 0.9 }}
            animate={{ opacity: 1, scale: 1 }}
            className="text-5xl md:text-7xl font-bold text-white mb-8 tracking-tight"
          >
            Kết Nối Tâm Hồn Việt Nam
          </motion.h1>
          <p className="text-xl md:text-2xl text-white/90 leading-relaxed max-w-3xl mx-auto font-light">
            Chúng tôi không chỉ bán tour, chúng tôi mang đến những hành trình chạm tới trái tim và khám phá vẻ đẹp bất tận của dải đất hình chữ S.
          </p>
        </div>
      </section>

      {/* Brand Story */}
      <section className="py-24 max-w-7xl mx-auto px-6">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-20 items-center">
          <div className="space-y-8">
            <div className="inline-block px-4 py-1.5 bg-green-50 text-green-700 font-bold text-xs tracking-widest uppercase rounded-full">
              Về Chúng Tôi
            </div>
            <h2 className="text-4xl md:text-5xl font-bold text-primary-dark leading-tight">
              VietnamVibe & Hành Trình Lan Tỏa Bản Sắc
            </h2>
            <div className="w-24 h-1.5 bg-accent rounded-full"></div>
            <div className="space-y-6 text-slate-600 text-lg leading-relaxed">
              <p>
                Khởi đầu từ niềm đam mê mãnh liệt với văn hóa bản địa, VietnamVibe được thành lập bởi những người con Việt Nam luôn khát khao kể câu chuyện về đất nước mình một cách chân thực nhất.
              </p>
              <p>
                Mỗi góc phố Hội An, mỗi cánh đồng lúa chín tại Mù Cang Chải hay những bữa cơm thân mật cùng người dân địa phương đều chứa đựng "Vibe" riêng biệt – một nhịp sống vừa hiện đại, vừa đậm đà bản sắc.
              </p>
            </div>
            <button 
              onClick={() => setView('tours')}
              className="flex items-center gap-3 border-2 border-primary text-primary px-8 py-4 rounded-2xl font-bold hover:bg-primary hover:text-white transition-all group"
            >
              Tìm hiểu thêm về hành trình
              <motion.span whileHover={{ x: 5 }} transition={{ type: "spring" }}>
                <ArrowRight className="w-5 h-5" />
              </motion.span>
            </button>
          </div>
          <div className="grid grid-cols-2 gap-4">
             <div className="space-y-4 mt-12">
               <img src="https://lh3.googleusercontent.com/aida-public/AB6AXuDXngVHnGP0YLbtOI0-CRN6yhpFqhiUr2NhR4adG9ztdbeER0buZPejoTOrlH5gC_iqIjxos1YPC0ajXzUT5Fn4Q9LJjAFB3SZOf-BjpljQQro0qoCgAKJo1QD8ha5OJkoN3kKqUudaNVF8uQYnxLcNlEWI-AP8vv5J6NEuxwTQtIynO-S2FqSNMb-kKwmjwSnnxwMw1YLL1RHb6Mx3JZivHazN0Wkl1moa7ASOMKElMbHLMDkEYzb6MCFQSxorEMfeIIu3t08cDg" className="rounded-3xl h-64 w-full object-cover shadow-2xl" alt="Vietnam 1" />
               <img src="https://lh3.googleusercontent.com/aida-public/AB6AXuCP7wFZgrShp8jL0wHN_qNpOh36YkmEQyz4fMkXMk7FpB1L84SNSsVIZ_hUfj8FNXI4jxCj-qxIQTbRzC5InHFk4MtTAhGAZ-LBP-1qURsFMTR1NtTlVU1FLaXLS8RRvQBv6QHaO9pOGE5fGFLFPXcUfTh2gLFxN3GSpN1HdNHFLHdxzND2499BBG4tir36vYK6skoDz9Cy9xSw9dt-dlbpte19rOM2Pczjg_2plXCt01tuP2dvcfx8DQIqxTCYCRkF8uvdDp_40Q" className="rounded-3xl h-48 w-full object-cover shadow-2xl" alt="Vietnam 2" />
             </div>
             <div className="space-y-4">
               <img src="https://lh3.googleusercontent.com/aida-public/AB6AXuDLGMBh9fF_vlo9b8DDEF2Su93VN-kgvKh6VKzpeGuZziA8sZW69Rd80poEq4ysJT1xti1TlT6XnBAs80SXvjlzJqQh436JqRgI82jUluEIjxkU-oyFCewD5ZJ43D1qTLRvVvKoZxWd69niTw0aqsq_f8HkDhgEShld-Q6mBgi2BvsPxdfcdS85cmyfWBHYyxJoPbjTRWmOcn3BfddkRIf9a3LGiMu_xNElfNqeqWrXTRUQzqgCcHlxkitv8lPOnSM--Sqe0BhOng" className="rounded-3xl h-48 w-full object-cover shadow-2xl" alt="Vietnam 3" />
               <img src="https://lh3.googleusercontent.com/aida-public/AB6AXuC9kUcJPCQULWcuzkHNC1rjAQHfYBwb-SpUjMJ-SDMzaNCJfRZ6YRx0a3dgiQR3ntuKMOhreoJVIWXHUuCCPDNMweqhScTWdW-4irzhu2CTJvDs1oTzsUWhqvqRoaXPiNYolMRyRJHPjuI-kaMpkwdq1kB5IJmexZzE21R_OnNkkabuhVwGg4u9dZYFFX0lpGlTtkGfe9ixYk65B6lenogmY3izBLWftlvO7353RezoQkT3oy18s-xIk0TyX62QtPvhutgZbDNhSg" className="rounded-3xl h-64 w-full object-cover shadow-2xl" alt="Vietnam 4" />
             </div>
          </div>
        </div>
      </section>

      {/* Stats Section */}
      <section className="bg-primary-dark py-20">
        <div className="max-w-7xl mx-auto px-6 grid grid-cols-2 lg:grid-cols-4 gap-12 text-center">
           <div className="space-y-2">
              <div className="text-white text-6xl font-black italic">15<span className="text-accent">+</span></div>
              <p className="text-white/60 font-bold uppercase tracking-widest text-sm">Năm kinh nghiệm</p>
           </div>
           <div className="space-y-2">
              <div className="text-white text-6xl font-black italic">500<span className="text-accent">+</span></div>
              <p className="text-white/60 font-bold uppercase tracking-widest text-sm">Tour độc bản</p>
           </div>
           <div className="space-y-2">
              <div className="text-white text-6xl font-black italic">50k<span className="text-accent">+</span></div>
              <p className="text-white/60 font-bold uppercase tracking-widest text-sm">Khách hàng</p>
           </div>
           <div className="space-y-2">
              <div className="text-white text-6xl font-black italic">4.9<span className="text-accent text-3xl">/5</span></div>
              <p className="text-white/60 font-bold uppercase tracking-widest text-sm">Hài lòng</p>
           </div>
        </div>
      </section>

      {/* Persuasive CTA */}
      <section className="py-24 bg-slate-50 relative overflow-hidden">
        <div className="max-w-7xl mx-auto px-6 text-center relative z-10">
          <div className="inline-flex items-center gap-2 mb-6 px-4 py-2 bg-green-100 rounded-full text-green-600 font-bold text-sm">
            <Star className="w-4 h-4 fill-green-600" />
            Bắt đầu hành trình của bạn ngay hôm nay
          </div>
          <h2 className="text-4xl md:text-5xl font-bold text-primary-dark mb-8 leading-tight">
            Sẵn sàng để Việt Nam kể cho bạn<br/>nghe những câu chuyện mới?
          </h2>
          <p className="text-lg text-slate-500 max-w-2xl mx-auto mb-12">
            Mỗi chuyến đi là một chương mới trong cuộc đời. Hãy để các chuyên gia của VietnamVibe giúp bạn viết nên những kỷ niệm đẹp nhất bằng sự tận tâm và am hiểu địa phương sâu sắc.
          </p>
          <div className="flex flex-col sm:flex-row justify-center items-center gap-6">
            <button className="w-full sm:w-auto bg-accent text-white px-10 py-5 rounded-2xl font-bold text-lg shadow-xl shadow-accent/30 hover:-translate-y-1 transition-all flex items-center justify-center gap-3">
              <Phone className="w-5 h-5" />
              Liên hệ tư vấn miễn phí
            </button>
            <button 
              onClick={() => setView('tours')}
              className="w-full sm:w-auto bg-white text-primary border-2 border-slate-200 px-10 py-5 rounded-2xl font-bold text-lg hover:bg-slate-100 transition-all"
            >
              Khám phá các tour độc bản
            </button>
          </div>
        </div>
      </section>
    </div>
  );
}
