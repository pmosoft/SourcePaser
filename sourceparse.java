import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;


public class SourceParser {

	public static void main(String[] args) {
		Test01 t1 = new Test01();
		t1.parsing01();
	}
}

class Test01 {
	
	public ArrayList<String> dirList = new ArrayList<String>();
	public String desti = "c:/workspace/rba/rca/result.sql";
	public String source = "c:/workspace/rba/rca";
	public File dir = new File(source);
	public File[] fileList = dir.listFiles();
	
	public BufferedWriter bw = null;
	public BufferedReader br = null;
	
	String sqlText = "";
	String sqlTables = "";
	String fileName = "";
	
	void parsing01() {
		try{
			for(int i=0;i<fileList.length;i++){
				File file = fileList[i];
				if(file.isFile()){
					fileName = file.getName();
					
					System.out.println("file="+file.getName());
					
					br = new BufferedReader(new InputStreamReader(new FileInputStream(source+"/"+file.getName())))
							;
					
					System.out.println("file2="+file.getName());
					
					while(true){
						String str=br.readLine();
						//System.out.println("str="+str);
						
						sqlText += str+"\n";
						if(str == null) break;
						
					}
					//sqlText="-----\n:;
					//sqlText+="/*asdfasfa*/\n:;
					//sqlText+="//aaa\n:;
					//sqlText+="FRA_CNTR_SAMP_FRM_DTLS , \n:;
					//sqlText+="FRA_CNTR_SAMP_FRM_DTLSAA , \n:;
					//sqlText+="FRA_CNTR_SAMP_FRM_DTLSBB , \n:;

					parsing();
					
					br.close();
				}
			}
		} catch(Exception e) {
			System.out.println("e="+e.getMessage());			
		}
		
	}
	
	void parsing() {
		Pattern p; Matcher m;
		
		String lineComment1 = "--.*";
		String lineComment2 = "//.*";
		String whitespace = "[\t\n]";
		String longComment = "/\\*([^*]|\\*+[^/*])*\\*+/";
	    String extTable01 = "(F|f)[a-zA-Z]{2}_[a-zA-Z_]+";
	    
	    //------------------------------------------------------------------------------
	    //                               Execute Parsing
	    //------------------------------------------------------------------------------
	    try{
	    	System.out.println("sqlText0="+sqlText);
	    	
		    //---------------------
		    // Del lineComment
		    //---------------------
	    	p = Pattern.compile(lineComment1);
	    	m = p.matcher(sqlText); sqlText = m.replaceAll("");
	    	System.out.println("sqlText1="+sqlText);

		    //---------------------
		    // Del whitespace
		    //---------------------
	    	p = Pattern.compile(whitespace);
	    	m = p.matcher(sqlText); sqlText = m.replaceAll("");
	    	System.out.println("sqlText2="+sqlText);
	    	
		    //---------------------
		    // Del longComment
		    //---------------------
	    	p = Pattern.compile(longComment);
	    	m = p.matcher(sqlText); sqlText = m.replaceAll("");
	    	System.out.println("sqlText3="+sqlText);
	    	
		    //---------------------
		    // ExtTable01
		    //---------------------
	    	p = Pattern.compile(extTable01);
	    	m = p.matcher(sqlText); 
	    	String extTab = "";
	    	
	    	bw = new BufferedWriter(new OutputStreamWriter(new FileOutputStream(desti)));
	    	
	    	boolean a=false;
	    	while(a=m.find()){
	    		System.out.println("["+m.start()+"_"+m.group()+"]");
	    		sqlTables = "insert into FRA_TABLES values('"+fileName+"','"+m.group()+"');";
	    		bw.write(sqlTables);
	    		bw.newLine();

	    	}
	    	
	    	bw.close();
	    } catch(Exception e) {
	    	System.out.println("e="+e.getMessage());
	    }

	}
}
