package de.ComputerElite;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Scanner;

import de.ComputerElite.zip.ZipIt;

public class Input {

	public static void main(String[] args) throws IOException {
		ArrayList<String> list = new ArrayList<>();
		ArrayList<String> content = new ArrayList<>();
		int overwritten = 0;
		int count = 0;
		int exported = 0;
		String Name = "";
		Scanner Scanner = new Scanner(System.in);
		System.out.print("Directory where the folders are located: ");
		String Source = Scanner.nextLine();
		System.out.print("Destination Folder: ");
		String dest = Scanner.nextLine();
		System.out.print("Do you want to overwrite existing zips? (y/n): ");
		String write = Scanner.nextLine();
		boolean overwrite = true;
		if(write.equalsIgnoreCase("n")) {
			overwrite = false;
		}
		if(dest.equalsIgnoreCase("")) {
			dest = Source;
		}
		Scanner.close();
		
		File file = new File(Source);
		String[] directories = file.list();
		
		
		for(int i = 0; i<directories.length; i++) {
			System.out.println("");
			if(directories[i].substring(directories[i].length()-4, directories[i].length()).equalsIgnoreCase(".zip")) {
				System.out.println("File "+directories[i]+" is already zipped");
				continue;
			}
			
			File Path = new File(Source+File.separator+directories[i]);
			if(!Path.isDirectory()) {
				System.out.println(directories[i]+" is no Song");
				continue;
			}
			
			File current = new File(Source+File.separator+directories[i]);
			String[] contents = current.list();
			content = new ArrayList<String>(Arrays.asList(contents));
			if(!content.contains("Info.dat") && !content.contains("info.dat")) {
				System.out.println(directories[i]+" is no Song");
				continue;
			}
			
			
			BufferedReader reader;
			try {
				reader = new BufferedReader(new FileReader(
				Source+File.separator+directories[i]+File.separator+"Info.dat" ));
				String line = reader.readLine();
				while (line != null) {
					//System.out.println(line);
					// read next line
					if(line.contains("songName")) {
						if(line.contains("version") && line.contains("songName")) {
							//BeatSage
							count = 0;
							Name = "";
							for(int n = 0; n<line.length(); n++) {
								if(count == 7) {
									Name = Name + line.substring(n, n+1);
								}
								if(line.substring(n, n+1).equalsIgnoreCase("\"")) {
									count++;
								}
							}
							Name = Name.substring(0, Name.length()-1);
							//Name = Name.replaceAll("[\\]", "");
							Name = Name.replaceAll("[/]", "");
							Name = Name.replaceAll("[:]", "");
							Name = Name.replaceAll("[*]", "");
							Name = Name.replaceAll("[?]", "");
							Name = Name.replaceAll("[\"]", "");
							Name = Name.replaceAll("[<]", "");
							Name = Name.replaceAll("[>]", "");
							Name = Name.replaceAll("[|]", "");
							for(int f = 0; f<Name.length(); f++) {
								if(Name.substring(f,f+1).equalsIgnoreCase("\\")) {
									Name = Name.substring(0, f-1) + Name.substring(f+1, Name.length());
								}
							}
							int Time = 0;
							while(Name.substring(Name.length()-1, Name.length()).equalsIgnoreCase(" ")) {
								Name = Name.substring(0, Name.length()-1);
							}
							while(list.contains(Name)) {
								Time++;
								if(Time > 1) {
									Name = Name.substring(0, Name.length()-1);
									Name = Name + Time;
								} else {
									Name = Name + " " + Time;
								}
								
							}
							list.add(Name);
							System.out.println("Song Name: "+Name);
							System.out.println("Folder name: "+directories[i]);
							File sour = new File(Source);
							File src = new File(sour+File.separator+directories[i]+File.separator);
							
							boolean exists = false;
							if(!overwrite) {
								File zip = new File(dest+File.separator+Name+".zip");
								if(zip.exists()) {
									exists = true;
									System.out.println("This Song already exists");
								}
							} else {
								File zip = new File(dest+File.separator+Name+".zip");
								if(zip.exists()) {
									overwritten++;
									System.out.println("Overwritten this Song");
								}
							}
							
							if(!exists) {
								ZipIt.zipDirectory(src, dest+File.separator+Name+".zip");
								exported++;
							}
							Name = "";
							src = new File("");
							
						} else {
							//normal Map
							count = 0;
							Name = "";
							for(int n = 0; n<line.length(); n++) {
								if(count == 3) {
										Name = Name + line.substring(n, n+1);
								}
								if(line.substring(n, n+1).equalsIgnoreCase("\"")) {
									count++;
								}
							}
							Name = Name.substring(0, Name.length()-1);
							//Name = Name.replaceAll("[\\]", "");
							Name = Name.replaceAll("[/]", "");
							Name = Name.replaceAll("[:]", "");
							Name = Name.replaceAll("[*]", "");
							Name = Name.replaceAll("[?]", "");
							Name = Name.replaceAll("[\"]", "");
							Name = Name.replaceAll("[<]", "");
							Name = Name.replaceAll("[>]", "");
							Name = Name.replaceAll("[|]", "");
							for(int f = 0; f<Name.length(); f++) {
								if(Name.substring(f,f+1).equalsIgnoreCase("\\")) {
									Name = Name.substring(0, f-1) + Name.substring(f+1, Name.length());
								}
							}
							int Time = 0;
							while(Name.substring(Name.length()-1, Name.length()).equalsIgnoreCase(" ")) {
								Name = Name.substring(0, Name.length()-1);
							}
							while(list.contains(Name)) {
								Time++;
								if(Time > 1) {
									Name = Name.substring(0, Name.length()-1);
									Name = Name + Time;
								} else {
									Name = Name + " " + Time;
								}
								
							}
							list.add(Name);
							System.out.println("Song Name: "+Name);
							System.out.println("Folder name: "+directories[i]);
							File sour = new File(Source);
							File src = new File(sour+File.separator+directories[i]+File.separator);
							
							boolean exists = false;
							if(!overwrite) {
								File zip = new File(dest+File.separator+Name+".zip");
								if(zip.exists()) {
									exists = true;
									System.out.println("This Song already exists");
								}
							} else {
								File zip = new File(dest+File.separator+Name+".zip");
								if(zip.exists()) {
									overwritten++;
									System.out.println("Overwritten this Song");
								}
							}
							
							if(!exists) {
								ZipIt.zipDirectory(src, dest+File.separator+Name+".zip");
								exported++;
							}
							Name = "";
							src = new File("");
							
						}
					}
					line = reader.readLine();
					
				}
				reader.close();
			} catch (IOException e) {
				e.printStackTrace();
			}
			
			
		}
		
		
		//ZipIt.zipDirectory(src, Output);;
		System.out.println("");
		System.out.println("");
		System.out.println("Finished! Exported "+exported+" Songs");
		if(overwrite) {
			System.out.println("Overwritten "+overwritten+" existing zips");
		}
	}

}
