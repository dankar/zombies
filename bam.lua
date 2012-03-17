

settings = NewSettings()

if family == "unix" then
	if platform == "macosx" then
		glfw = Collect("lib/glfw/*.c", "lib/glfw/macosx/*.c")	
	else
		settings.cc.flags:Add("-L/usr/X11R6/lib -Llib/fmod/")
		settings.link.libs:Add("X11")
		settings.link.libs:Add("Xxf86vm")
		settings.link.libs:Add("GL")
		settings.link.libs:Add("GLU")
		settings.link.libs:Add("pthread")
		settings.link.libs:Add("glfw")
		settings.link.libs:Add("png")
		settings.link.libs:Add("z")
		settings.link.libs:Add("GLEW")
		settings.link.libs:Add("expat")
		settings.link.libs:Add("scew")
		settings.link.libs:Add("ode")
			
			
			
			
			
			

	end
elseif family == "windows" then
	settings.cc.defines:Add("WIN32")
	settings.cc.defines:Add("_WINDOWS")
	settings.cc.defines:Add("HAVE_MEMMOVE")
	settings.cc.defines:Add("XML_STATIC")
	settings.cc.flags:Add("/EHsc /wd4005")
	settings.link.flags:Add("/OPT:REF")
	settings.link.libs:Add("opengl32")
	settings.link.libs:Add("glu32")
	settings.link.libs:Add("gdi32")
	settings.link.libs:Add("user32")
	settings.link.libs:Add("wsock32")
	glfw = Collect("../zombies-ext/lib/glfw/*.c", "../zombies-ext/lib/glfw/win32/*.c")
end


settings.cc.defines:Add("GLEW_STATIC")

--settings.cc.defines:Add("_PROFILING")


settings.cc.includes:Add("src/")
src = Collect("src/*.cpp")
src_vid = Collect("src/video/*.cpp")
src_math = Collect("src/math/*.cpp")

gamesrc = Collect("gamesrc/*.cpp")

objs = Compile(settings, src, src_vid, src_math, gamesrc, png, zlib, glew, glfw, opcode, ode, scew, expat)
exe = Link(settings, "bin/mecha", objs)

